using Ruley.NET;
using System;

namespace TestApp
{
    class Program
	{
		static void Main(string[] args)
		{
			var p = YamlParser.LoadFromFile(@"rules/graphite.yaml");
			p.Start();
			Console.ReadLine();
		}
	}
}
