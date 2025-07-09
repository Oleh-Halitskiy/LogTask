using LogComponent.Tests.Mocks;
using LogTest.Core;
using LogTest.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace LogComponent.Tests
{
    public class LogManagerTests : IDisposable
    {
        private readonly string _tempDirectory = Path.Combine(Path.GetTempPath() + "LogManagerTests") + Path.DirectorySeparatorChar;
        private readonly ITestOutputHelper output;

        public LogManagerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }

        private bool FileContainsString(string path, string content)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string contents = sr.ReadToEnd();
                if (contents.Contains(content))
                {
                    return true;
                }
                return false;
            }
        }

        [Fact]
        public void MainLoop_ShouldCreateNewLogFileWhenDayChanges()
        {
            var mockClock = new MockClock(new DateTime(2022, 1, 1, 23, 59, 59));
            
            var logManager = new LogManager(_tempDirectory, mockClock);
            
            logManager.WriteLog("Test");
            
            Thread.Sleep(500);
            
            string initialLogPath = logManager.CurrentLogPath;
            
            mockClock.Now = new DateTime(2022, 1, 2, 0, 0, 0);
            
            logManager.WriteLog("Test2");

            Thread.Sleep(500);

            Assert.NotEqual(initialLogPath, logManager.CurrentLogPath);
        }

        [Fact]
        public void MainLoop_ShouldWriteSomething()
        {
            var mockClock = new MockClock(new DateTime(2022, 1, 1, 23, 59, 50));
            var logManager = new LogManager(_tempDirectory, mockClock);

            var log1 = "TestFirstLog";
            var log2 = "TestSecondLog";
            var log3 = "TestThirdLog";
            
            logManager.WriteLog(log1);
            logManager.WriteLog(log2);
            logManager.WriteLog(log3);
            
            Thread.Sleep(500);
            
            string logPath = logManager.CurrentLogPath;

            logManager.Stop();
            
            Assert.True(FileContainsString(logPath, log1));
            Assert.True(FileContainsString(logPath, log2));
            Assert.True(FileContainsString(logPath, log3));
        }
        
        [Fact]
        public void Stop_ShouldStopWithFlush()
        {
            var logManager = new LogManager(_tempDirectory);

            for (int i = 0; i < 5; i++)
            {
                logManager.WriteLog($"Test{i}");
            }
            
            logManager.StopWithFlush();
            
            Thread.Sleep(1000);
            
            for (int i = 0; i < 5; i++)
            {
                Assert.True(FileContainsString(logManager.CurrentLogPath, $"Test{i}"));
            }
        }

        [Fact]
        public void Stop_ShouldStopWithoutFlush()
        {
            var logManager = new LogManager(_tempDirectory);

            for (int i = 0; i < 5; i++)
            {
                logManager.WriteLog($"Test{i}");
            }

            logManager.Stop();

            Thread.Sleep(1000);

            var lines = File.ReadAllLines(logManager.CurrentLogPath).ToList();
            
            Assert.NotEqual(6, lines.Count);
            Assert.Equal(2, lines.Count);
        }

        [Fact]
        public void Add_ShouldNotAllowToAddAfterStop()
        {
            var logManager = new LogManager(_tempDirectory);

            output.WriteLine(_tempDirectory);

            logManager.Stop();

            Assert.Throws<LogNotAcceptedException>(() => logManager.WriteLog("Something"));
        }
        
    }
}
