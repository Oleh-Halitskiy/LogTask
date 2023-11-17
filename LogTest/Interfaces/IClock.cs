using System;

namespace LogTest.Interfaces
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}