using Ruley.Core;
using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Ruley;

namespace TestApp
{
    public class Test
    {
        public int A { get; set; }
        public int B { get; set; }

        public DynamicDictionary[] Parameters { get; set; }
        public object Definition { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //NLog.LogManager.GetLogger("dsfsdf").Debug("TEST");
            //var rm = new RuleManager();
            //rm.Start();
            //Console.ReadLine();

            var s = File.ReadAllText(@"rules/test.yaml");

            var input = new StringReader(s);
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            var order = deserializer.Deserialize<Test>(input);

            var p = Pipeline.FromDynamic(order.Definition);

            p.Subscribe(x =>
            {
                Console.WriteLine("dfsdfdsf");
            }, (Exception e) => Console.WriteLine(e));

            p.Start();

            Console.ReadLine();
        }
    }
}
