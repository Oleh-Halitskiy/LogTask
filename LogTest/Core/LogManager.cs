using System;
using System.Collections.Generic;
using System.Threading;
using LogTest.Exceptions;
using LogTest.Utils;
using LogTest.Interfaces;

namespace LogTest.Core
{

    public class LogManager : ILogManager
    {
        private Thread _runThread;
        private Queue<ILogLine> _logQueue;
        private DateTime _currentDate;
        private IFileManager _fileManager;

        private bool _acceptingNewLogs;
        private bool _exit;
        private string _currentDirectoryPath;
        private string _currentLogPath;
        private string _crashLogPath;

        private readonly string _header = "Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ');
        private readonly IClock _clock = new SystemClock();
        private const string _stringFormat = "yyyyMMdd HHmmss fff";

        /// <inheritdoc />
        public bool AcceptingNewLogs { get => _acceptingNewLogs; }
        /// <inheritdoc />
        public Queue<ILogLine> LogQueue { get => _logQueue; }
        /// <summary>
        /// Path where crash log file will be created in case of erros, set to CurrentLogPath by default when processing exception
        /// </summary>
        public string CrashLogPath { get => _crashLogPath; set => _crashLogPath = value; }
        /// <inheritdoc />
        public string CurrentLogPath { get => _currentLogPath; set => _currentLogPath = value; }

        /// <summary>
        /// Creates new LogManeger
        /// </summary>
        /// <param name="logDirectory">Directory where to create logs</param>
        public LogManager(string logDirectory)
        {
            _acceptingNewLogs = true;
            _exit = true;

            _fileManager = new FileManager();
            _logQueue = new Queue<ILogLine>();

            _currentDate = _clock.Now;
            _currentDirectoryPath = logDirectory;

            CreateNewLogFile(_currentDate.ToString(_stringFormat) + ".log");

            // run main loop in thread
            _runThread = new Thread(MainLoop);
            _runThread.Start();
        }

        /// <summary>
        /// Optional constructor to set custom IClock
        /// </summary>
        /// <param name="logDirectory">Directory where to create logs</param>
        /// <param name="clock">Custom clock</param>
        public LogManager(string logDirectory, IClock clock):this(logDirectory)
        {
            _clock = clock;
            _currentDate = clock.Now;                                                                                                  
        }

        /// <inheritdoc />
        public void Stop(bool stopWithFlush = true)
        {
            if (stopWithFlush)
            {
                _acceptingNewLogs = false;
            }
            else
            {
                _exit = false;
                _acceptingNewLogs = false;
            }
        }

        /// <inheritdoc />
        public void WriteLog(string text)
        {
            if (_acceptingNewLogs)
            {
                ILogLine logLine = new LogLine(text, DateTime.Now);
                _logQueue.Enqueue(logLine);
            }
            else
            {
                throw new LogNotAcceptedException("Log manager is stopped and not accepting new logs");
            }
        }

        /// <summary>
        /// Main loop that performs all the checking and processing of logs
        /// </summary>
        private void MainLoop()
        {
            while (_exit)
            {
                try
                {
                    if (!_acceptingNewLogs && _logQueue.Count == 0)
                    {
                        break;
                    }
                    if (_currentDate.Day + 1 == _clock.Now.Day)
                    {
                        _currentDate = _clock.Now;
                        CreateNewLogFile(_currentDate.ToString(_stringFormat) + ".log");
                    }
                    if (_logQueue.Count > 0)
                    {
                        ProcessLog();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    break;
                }
                Thread.Sleep(100);
            }

        }

        /// <summary>
        /// Function to handle exception in main loop
        /// </summary>
        /// <param name="ex">Exception that'll be processed</param>
        private void HandleException(Exception ex)
        {
            _crashLogPath = _crashLogPath ?? _currentDirectoryPath; // save logs by default to the logs directory
            var crashLogName = $"Exceptions{DateTime.Now.ToString(_stringFormat)}.log";

            _fileManager.CreateFile(_crashLogPath, crashLogName);

            _fileManager.WriteToFile(_crashLogPath + crashLogName, $"Crash occured at {DateTime.Now}\n" +
                                                                   $"Error: {ex.Message}\n" +
                                                                   "\n");
        }

        /// <summary>
        /// Creating new log file, used for when we need to have new file when passing midnight or in general when we need file for logs
        /// </summary>
        /// <param name="fileName"></param>
        private void CreateNewLogFile(string fileName)
        {
            _fileManager.CreateFile(_currentDirectoryPath, fileName);
            _currentLogPath = _currentDirectoryPath + fileName;
            _fileManager.WriteToFile(_currentLogPath, _header);
        }

        /// <summary>
        /// Actually write logs to the file and removes it from the queue
        /// </summary>
        private void ProcessLog()
        {
            _fileManager.WriteToFile(_currentLogPath, _logQueue.Peek().ToString());
            _logQueue.Dequeue();
        }
    }
}