using LogTest.Interfaces;
using System;

namespace LogComponent.Tests.Mocks
{
    internal class MockClock : IClock
    {
        public DateTime Now { get; set; }

        public MockClock(DateTime initialDateTime)
        {
            Now = initialDateTime;
        }
    }
}
