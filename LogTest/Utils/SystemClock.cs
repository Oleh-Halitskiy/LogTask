using System;

namespace LogTest.Interfaces
{
    /// <summary>
    /// System clocks that serves as Wrapper to DateTime.Now
    /// </summary>
    public class SystemClock : IClock
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.Now;
    }
}
