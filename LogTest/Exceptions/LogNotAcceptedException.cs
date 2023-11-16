using System;

namespace LogTest.Exceptions
{
    internal class LogNotAcceptedException : Exception
    {
        public LogNotAcceptedException()
        {
        }

        public LogNotAcceptedException(string message) : base(message)
        {
        }

        public LogNotAcceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}