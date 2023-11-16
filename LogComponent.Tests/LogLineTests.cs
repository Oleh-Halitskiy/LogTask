using LogTest.Core;
using System;
using Xunit;

namespace LogComponent.Tests
{
    public class LogLineTests
    {
        [Fact]
        public void DefaultConstructor_ShouldInitializeProperties()
        {
            var logLine = new LogLine();

            Assert.Equal("", logLine.Text);
            Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logLine.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }

        [Fact]
        public void ParameterizedConstructor_ShouldSetProperties()
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
    }
}
