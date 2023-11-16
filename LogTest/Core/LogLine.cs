using LogTest.Interfaces;
using System;

namespace LogTest.Core
{
    /// <summary>
    /// This is the object that the diff. loggers (filelogger, consolelogger etc.) will operate on. The LineText() method will be called to get the text (formatted) to log
    /// </summary>
    public class LogLine : ILogLine
    {
        private string _text;
        private DateTime _timeStamp;

        public string Text { get => _text; set => _text = value; }
        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }

        public LogLine()
        {
            _text = "";
            _timeStamp = DateTime.Now;
        }
        public LogLine(string text, DateTime timeStamp)
        {
            _text = text;
            _timeStamp = timeStamp;
        }
        public override string ToString()
        {
            string timeStampString = _timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff");
            return $"{timeStampString}: {_text}";
        }
    }
}