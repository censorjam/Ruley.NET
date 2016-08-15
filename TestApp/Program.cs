using System;
using Ruley;

namespace TestApp
{

    class Program
	{
		static void Main(string[] args)
		{
			var p = YamlParser.Load(@"rules/graphite.yaml");

			//p.Subscribe(x =>
			//{
			//}, (Exception e) => Console.WriteLine(e));

			p.Start();

			Console.ReadLine();
		}
	}
}
