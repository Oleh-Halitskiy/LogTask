using LogComponent.Tests.Mocks;
using LogTest.Core;
using LogTest.Exceptions;
using System;
using System.IO;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace LogComponent.Tests
{
    public class LogManagerTests : IDisposable
    {
        private readonly string _tempDirectory = Path.GetTempPath() + "LogManagerTests\\";
        private readonly ITestOutputHelper output;

        public LogManagerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            Directory.Delete(_tempDirectory, true);
        }

        public bool FileContainsString(string path, string content)
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
            string initialLogPath = logManager.CurrentLogPath;


            mockClock.Now = new DateTime(2022, 1, 2, 0, 0, 0);
            Thread.Sleep(200);

            Assert.NotEqual(initialLogPath, logManager.CurrentLogPath);
        }

        [Fact]
        public void MainLoop_ShouldWriteSomething()
        {
            var mockClock = new MockClock(new DateTime(2022, 1, 1, 23, 59, 50));
            var logManager = new LogManager(_tempDirectory, mockClock);
            string logPath = logManager.CurrentLogPath;

            var log1 = "TestFirstLog";
            var log2 = "TestSecondLog";
            var log3 = "TestThirdLog";


            logManager.WriteLog(log1);
            logManager.WriteLog(log2);
            logManager.WriteLog(log3);

            logManager.Stop();
            Thread.Sleep(400);

            Assert.True(FileContainsString(logPath, log1));
            Assert.True(FileContainsString(logPath, log2));
            Assert.True(FileContainsString(logPath, log3));
        }

        [Fact]
        public void Stop_ShouldStopWithoutFlush()
        {
            var logManager = new LogManager(_tempDirectory);

            for (int i = 0; i < 5; i++)
            {
                logManager.WriteLog($"Test{i}");
            }
            logManager.Stop(false); // stopping without flush
            Thread.Sleep(600);

            Assert.NotEmpty(logManager.LogQueue);

        }

        [Fact]
        public void Stop_ShouldStopWithFlush()
        {
            var logManager = new LogManager(_tempDirectory);

            for (int i = 0; i < 5; i++)
            {
                logManager.WriteLog($"Test{i}");
            }
            logManager.Stop(true);
            Thread.Sleep(600);

            Assert.Empty(logManager.LogQueue);
        }

        [Fact]
        public void Add_ShouldNotAllowToAddAfterStop()
        {
            var logManager = new LogManager(_tempDirectory);

            logManager.Stop();

            Assert.Throws<LogNotAcceptedException>(() => logManager.WriteLog("Something"));
            Assert.False(logManager.AcceptingNewLogs);
        }
        [Fact]
        public void HandleException_ShouldSetCrashLogToLogPathByDefault()
        {
            
            var logManager = new LogManager(_tempDirectory);
            logManager.WriteLog("TestWrite");

            File.Delete(logManager.CurrentLogPath);

            logManager.Stop();

            Thread.Sleep(300);

            Assert.Equal(logManager.CrashLogPath, _tempDirectory);
        }
        [Fact]
        public void HandleException_NotOverridingSetCrashLogPath()
        {
            var logManager = new LogManager(_tempDirectory);
            logManager.CrashLogPath = _tempDirectory;

            logManager.WriteLog("TestWrite");

            File.Delete(logManager.CurrentLogPath);

            logManager.Stop();

            Thread.Sleep(300);

            Assert.Equal(logManager.CrashLogPath, _tempDirectory);
        }
    }
}
