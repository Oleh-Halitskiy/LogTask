using System.Collections.Generic;

namespace LogTest.Interfaces
{
    public interface ILogManager
    {
        bool AcceptingNewLogs { get; }
        string CrashLogPath { get; set; }
        Queue<ILogLine> LogQueue { get; }

        void Stop(bool stopWithFlush = true);
        void WriteLog(string text);
    }
}