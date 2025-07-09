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
        private DateTime _currentDate;
        private bool _acceptingNewLogs;
        private bool _exit;
        private string _currentLogPath;
        private string _crashLogPath;
        
        private readonly IFileManager _fileManager;
        private readonly Queue<ILogLine> _logQueue;
        private readonly string _currentDirectoryPath;
        private readonly string _header = "Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ');
        private readonly IClock _clock = new SystemClock();
        private const string _stringFormat = "yyyyMMdd HHmmss fff";
        
        public string CrashLogPath { get => _crashLogPath; set => _crashLogPath = value; }
        public string CurrentLogPath => _currentLogPath;

        public LogManager(string logDirectory)
        {
            _acceptingNewLogs = true;
            _exit = true;

            _fileManager = new FileManager();
            _logQueue = new Queue<ILogLine>();

            _currentDate = _clock.Now;
            _currentDirectoryPath = logDirectory;
            _currentLogPath = string.Empty;
            
            _crashLogPath = _currentDirectoryPath;
            
            var runThread = new Thread(MainLoop)
            {
                IsBackground = true
            };
            runThread.Start();
        }
        
        public LogManager(string logDirectory, IClock clock):this(logDirectory)
        {
            _clock = clock;
            _currentDate = clock.Now;                                                                                                  
        }
        
        public void Stop()
        {
           _acceptingNewLogs = false;
           _exit = false;
        }
        
        public void StopWithFlush()
        {
            _acceptingNewLogs = false;
        }

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
                        _currentLogPath = string.Empty;
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
            }

        }
        
        private void HandleException(Exception ex)
        {
            var crashLogName = $"Exceptions{DateTime.Now.ToString(_stringFormat)}.log";
            
            _fileManager.CreateFile(_crashLogPath, crashLogName);
            _fileManager.AppendLine(_crashLogPath + crashLogName, $"Crash occured at {DateTime.Now}\n" +
                                                                   $"Error: {ex.Message}\n" +
                                                                   "\n");
        }
        
        private void CreateNewLogFile(string fileName)
        {
            _fileManager.CreateFile(_currentDirectoryPath, fileName);
            _currentLogPath = _currentDirectoryPath + fileName;
            _fileManager.AppendLine(_currentLogPath, _header);
        }
        
        private void ProcessLog()
        {
            if (_currentLogPath == string.Empty)
            {
                CreateNewLogFile(_currentDate.ToString(_stringFormat) + ".log");
            }
            object lockObject = new object();
            lock (lockObject)
            {
                _fileManager.AppendLine(_currentLogPath, _logQueue.Peek().ToString());
                _logQueue.Dequeue();
            }
        }
    }
}