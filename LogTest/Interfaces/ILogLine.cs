using System;

namespace LogTest.Interfaces
{
    public interface ILogLine
    {
        string Text { get; set; }
        DateTime TimeStamp { get; set; }
        
    }
}