using Ruley;
using Ruley.NET;
using Ruley.NET.External;
using System;
using System.Linq;

namespace TestApp
{
    class Program
    {
        private static Logger Logger;

        static void Main(string[] args)
        {
            Logger = new Logger()
            {
                Name = "Application"
            };

            AppDomain.CurrentDomain.UnhandledException += (a, b) =>
            {
                Logger.Error((Exception)b.ExceptionObject);
            };

            IoC.Register<ITimeProvider>(new UtcTimeProvider());
            IoC.Register<IConsoleOutput>(new SystemConsole());
            IoC.Register<IUdpOutClient>(new TestUdpOutClient());
            IoC.Register<IEmailClient>(new EmailClient());

            var p = YamlParser.LoadFromFile(@"rules/graphite.yaml");
            p.ToList().ForEach(x => x.Start());

            Console.ReadLine();
        }
    }
}
