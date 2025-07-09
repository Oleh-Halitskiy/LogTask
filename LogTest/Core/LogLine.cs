using LogTest.Interfaces;
using System;

namespace LogTest.Core
{
    public class LogLine : ILogLine
    {
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        
        public LogLine(string text, DateTime timeStamp)
        {
            Text = text;
            TimeStamp = timeStamp;
        }

        public override string ToString()
        {
            return TimeStamp.ToString() + "" + Text;
        }
    }
}