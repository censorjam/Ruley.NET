using Ruley.Core;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var rm = new RuleManager();
            rm.Start();
            Console.ReadLine();
        }
    }
}
