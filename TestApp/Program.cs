using Ruley;
using Ruley.NET;
using Ruley.NET.External;
using System;

namespace TestApp
{
    class Program
    {
        private static Logger Logger = new Logger("Application", false);

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (a, b) =>
            {
                Logger.Error((Exception)b.ExceptionObject);
            };

            IoC.Register<IConsoleOutput>(new SystemConsole());
            IoC.Register<IUdpOutClient>(new TestUdpOutClient());

            var p = YamlParser.LoadFromFile(@"rules/graphite.yaml");
            p.Start();
            Console.ReadLine();
        }
    }
}
