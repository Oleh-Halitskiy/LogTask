using LogTest.Interfaces;
using System;

namespace LogTest.Core
{
    /// <summary>
    /// Log
    /// </summary>
    public class LogLine : ILogLine
    {
        private string _text;
        private string _dateFormat;
        private DateTime _timeStamp;

        /// <inheritdoc />
        public string Text { get => _text; set => _text = value; }
        /// <inheritdoc />
        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }

        /// <summary>
        /// Creates new LogLine object
        /// </summary>
        /// <param name="text">Sets the Text property</param>
        /// <param name="timeStamp">Sets the TimeStamp</param>
        /// <param name="dateFormat">Optional parameter to have specific format for timestamp</param>
        public LogLine(string text, DateTime timeStamp, string dateFormat = "yyyy-MM-dd HH:mm:ss:fff")
        {
            _text = text;
            _timeStamp = timeStamp;
            _dateFormat = dateFormat;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string timeStampString = _timeStamp.ToString(_dateFormat);
            return $"{timeStampString}: {_text}";
        }
    }
}