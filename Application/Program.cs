using System;
using System.Threading;
using LogTest.Core;
using LogTest.Interfaces;
using LogTest.Utils;

namespace LogUsers
{
    class Program
    {
        static void Main(string[] args)
        {

            ILog  logger = new LogManager("C:\\Users\\user\\Desktop\\Logger\\Logs\\");

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Console.WriteLine("Writing to logger");
                Thread.Sleep(50);
            }

            logger.StopWithFlush();


            /*
            ILog logger2 = new LogManager("C:\\Users\\user\\Desktop\\Logger\\Logs");

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger2.StopWithoutFlush();

            Console.ReadLine();
            */
        }
    }
}
