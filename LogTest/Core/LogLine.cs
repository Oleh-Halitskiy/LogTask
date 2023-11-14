using System;
using System.Text;

namespace LogTest.Core
{
    /// <summary>
    /// This is the object that the diff. loggers (filelogger, consolelogger etc.) will operate on. The LineText() method will be called to get the text (formatted) to log
    /// </summary>
    public class LogLine
    {
        private string text;
        private DateTime timeStamp;

        public string Text { get => text; set => text = value; }
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }

        public LogLine()
        {
            text = "";
            timeStamp = DateTime.Now;
        }
        public LogLine(string text, DateTime timeStamp)
        {
            this.text = text;
            this.timeStamp = timeStamp;
        }
        public override string ToString()
        {
            string timeStampString = timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff");
            return $"{timeStamp}: {text}";
        }
    }
}