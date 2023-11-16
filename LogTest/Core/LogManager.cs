using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public bool AcceptingNewLogs { get => _acceptingNewLogs; }
        public Queue<ILogLine> LogQueue { get => _logQueue; }
        public string CrashLogPath { get => _crashLogPath; set => _crashLogPath = value; }

        public LogManager(string logDirectory)
        {
            _acceptingNewLogs = true;
            _exit = true;

            _fileManager = new FileManager();
            _logQueue = new Queue<ILogLine>();

            _currentDate = DateTime.Now;
            _currentDirectoryPath = logDirectory;

            CreateNewLogFile(_currentDate.ToString("yyyyMMdd HHmmss fff") + ".log");

            // run main loop in thread
            _runThread = new Thread(MainLoop);
            _runThread.Start();
        }

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
                    if (_currentDate.Day + 1 == DateTime.Now.Day)
                    {
                        _currentDate = DateTime.Now;
                        CreateNewLogFile(_currentDate.ToString("yyyyMMdd HHmmss fff") + ".log");
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
                Thread.Sleep(1000);
            }

        }

        private void HandleException(Exception ex)
        {
            var crashLogName = $"Exceptions{DateTime.Now.ToString("yyyyMMdd-HHmm")}.log";
            _fileManager.CreateFile(_crashLogPath, crashLogName);

            _fileManager.WriteToFile(_crashLogPath + crashLogName, $"Crash occured at {DateTime.Now}\n" +
                                                                   $"Error: {ex.Message}\n" +
                                                                   "\n");
        }

        private void CreateNewLogFile(string fileName)
        {
            _fileManager.CreateFile(_currentDirectoryPath, fileName);
            _currentLogPath = _currentDirectoryPath + fileName;
            _fileManager.WriteToFile(_currentLogPath, _header);
        }

        private void ProcessLog()
        {
            _fileManager.WriteToFile(_currentLogPath, _logQueue.Peek().ToString());
            _logQueue.Dequeue();
        }
    }
}