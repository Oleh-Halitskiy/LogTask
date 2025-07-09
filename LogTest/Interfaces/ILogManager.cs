using System.Collections.Generic;

namespace LogTest.Interfaces
{
    public interface ILogManager
    {
        string CrashLogPath { get; set; }
        
        void Stop();
        void StopWithFlush();
        void WriteLog(string text);
    }
}