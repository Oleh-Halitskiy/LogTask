using System;

namespace LogTest.Interfaces
{
    /// <summary>
    /// LogLine serves to be object that we use in LogLine, containing all core information like Text and Timestamp
    /// </summary>
    public interface ILogLine
    {
        /// <summary>
        /// Text of log
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// TimeStamp of log which is TimeDate datatype
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// To string method, that converts LogLine into string
        /// </summary>
        /// <returns>String containing all core information in readable manner</returns>
        string ToString();
    }
}