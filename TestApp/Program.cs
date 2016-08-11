using Ruley.Core;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NLog.LogManager.GetLogger("dsfsdf").Debug("TEST");

            var rm = new RuleManager();
            rm.Start();
            Console.ReadLine();
        }
    }
}
