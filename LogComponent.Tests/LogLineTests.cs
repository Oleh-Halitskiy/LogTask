using LogTest.Core;
using System;
using Xunit;

namespace LogComponent.Tests
{
    public class LogLineTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            var text = "Test log entry";
            var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);

            var logLine = new LogLine(text, timeStamp);

            Assert.Equal(text, logLine.Text);
            Assert.Equal(timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff"), logLine.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var text = "Test log entry";
            var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
            var logLine = new LogLine(text, timeStamp);

            var result = logLine.ToString();

            var expectedString = $"{timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff")}: {text}";
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToString_ShouldUseDefaultDateFormat()
        {
            var text = "Test log entry";
            var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
            var logLine = new LogLine(text, timeStamp);

            var result = logLine.ToString();

            var expectedString = $"{timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff")}: {text}";
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToString_ShouldUseCustomDateFormat()
        {
            var text = "Test log entry";
            var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
            var dateFormat = "MM/dd/yyyy HH:mm:ss";
            var logLine = new LogLine(text, timeStamp, dateFormat);

            var result = logLine.ToString();

            var expectedString = $"{timeStamp.ToString(dateFormat)}: {text}";
            Assert.Equal(expectedString, result);
        }
    }
}
