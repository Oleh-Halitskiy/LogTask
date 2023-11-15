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
    public class LogManager : ILog
    {
        private Thread _runThread;
        private Queue<LogLine> _logQueue = new Queue<LogLine>();
        private bool _acceptingNewLogs;
        private DateTime startDate;
        private string _currentDirectoryPath;
        private string _currentLogPath;
        private string _crashLogPath;

        private IFileManager _fileManager;
 
        private bool _exit;

        public bool AcceptingNewLogs { get => _acceptingNewLogs; set => _acceptingNewLogs = value; }
        public Queue<LogLine> LogQueue { get => _logQueue; set => _logQueue = value; }
        public string CrashLogPath { get => _crashLogPath; set => _crashLogPath = value; }

        public LogManager(string logDirectory)
        {
            _acceptingNewLogs = true;
            _exit = true;
            _fileManager = new FileManager();
            startDate = DateTime.Now;
            _currentDirectoryPath = logDirectory;

            CreateNewLogFile(startDate.ToString("yyyyMMdd HHmmss fff") + ".log");

            // run main loop in thread
            _runThread = new Thread(MainLoop2);
            _runThread.Start();
        }

        private void MainLoop2()
        {
            while (_exit)
            {
                try
                {
                    if (!_acceptingNewLogs && _logQueue.Count == 0)
                    {
                        break;
                    }
                    if (startDate.Day + 1 == DateTime.Now.Day)
                    {
                        startDate = DateTime.Now;
                        CreateNewLogFile(startDate.ToString("yyyyMMdd HHmmss fff") + ".log");
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
        }
        private void ProcessLog()
        {
            _fileManager.WriteToFile(_currentLogPath, _logQueue.Peek().ToString());
            _logQueue.Dequeue();
        }
        public void StopWithoutFlush()
        {
            _exit = false;
        }

        public void StopWithFlush()
        {
            _acceptingNewLogs = false;
        }

        public void Write(string text)
        {
            if (_acceptingNewLogs)
            {
                LogLine logLine = new LogLine(text, DateTime.Now);
                _logQueue.Enqueue(logLine);
            }
            else
            {
                throw new LogNotAcceptedException("Log manager is stopped and not accepting new logs"); 
            }
        }
    }
}