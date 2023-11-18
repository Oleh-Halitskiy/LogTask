using System.Collections.Generic;

namespace LogTest.Interfaces
{
    /// <summary>
    /// Core class that performs all operations regarding logs, like accepting logs to queue, creating new log files, writing actual logs to the files
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Bool that indicates if manager accepting new logs, set to false after log manager stopped
        /// </summary>
        bool AcceptingNewLogs { get; }

        /// <summary>
        /// Crash log path in case of exceptions, the file with error info will be created there 
        /// </summary>
        string CrashLogPath { get; set; }

        /// <summary>
        /// Queue of logs currently in log manager
        /// </summary>
        Queue<ILogLine> LogQueue { get; }

        /// <summary>
        /// Stop the log manager from printing logs and accepting new logs
        /// </summary>
        /// <param name="stopWithFlush">If set to true, all outstanding logs will be still be processed, if false then stops immediately</param>
        void Stop(bool stopWithFlush = true);

        /// <summary>
        /// Writes log to the specified directory
        /// </summary>
        /// <param name="text">Content to write</param>
        void WriteLog(string text);
    }
}