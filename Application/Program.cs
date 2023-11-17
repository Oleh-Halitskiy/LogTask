using System;
using System.IO;
using System.Threading;
using LogTest.Core;
using LogTest.Interfaces;

namespace LogUsers
{
    class Program
    {
        static void Main(string[] args)
        {

            var currentDirectory = Environment.CurrentDirectory + @"\Logs\";
            ILogManager logger = new LogManager(currentDirectory);
            logger.CrashLogPath = currentDirectory;

            for (int i = 0; i < 15; i++)
            {
                logger.WriteLog("Number with Flush: " + i.ToString());
            }

            logger.Stop();

            ILogManager logger2 = new LogManager(currentDirectory);

            for (int i = 50; i > 0; i--)
            {
                logger2.WriteLog("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger2.Stop(false);

            Console.ReadLine();
        }
    }
}
