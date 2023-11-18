using System;

namespace LogTest.Exceptions
{
    /// <summary>
    /// Exception class to throw custom exception when LogManager can't accept new logs after we stop it
    /// </summary>
    public class LogNotAcceptedException : Exception
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