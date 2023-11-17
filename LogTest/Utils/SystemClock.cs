using System;

namespace LogTest.Interfaces
{
    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}
