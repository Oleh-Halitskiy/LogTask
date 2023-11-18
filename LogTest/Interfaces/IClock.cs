using System;

namespace LogTest.Interfaces
{
    /// <summary>
    /// This little Clock class could be part of LogManager, so that we can pass custom clocks there
    /// In LogManager, file names and log content depends on what time it is
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Now property that will return current time
        /// </summary>
        DateTime Now { get; }
    }
}