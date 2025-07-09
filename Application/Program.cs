using System;
using System.IO;
using LogTest.Core;
using LogTest.Interfaces;

namespace LogUsers
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Path.Combine(Environment.CurrentDirectory, "Logs") + Path.DirectorySeparatorChar;
            Console.WriteLine($"Current directory: {currentDirectory}");

            ILogManager logger = new LogManager(currentDirectory);
            logger.CrashLogPath = currentDirectory;

            for (int i = 0; i < 100; i++)
            {
                logger.WriteLog("Number with Flush: " + i.ToString());
            }

            logger.StopWithFlush();

            ILogManager logger2 = new LogManager(currentDirectory);

            for (int i = 500; i > 0; i--)
            {
                logger2.WriteLog("Number with No flush: " + i.ToString());
            }

            logger2.Stop();
            Console.ReadKey();
        }
    }
}
