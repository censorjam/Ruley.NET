using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Ruley;

namespace TestApp
{

	class Program
	{
		static void Main(string[] args)
		{
			var p = YamlParser.Load(@"rules/test.yaml");

			p.Subscribe(x =>
			{
				Console.WriteLine("dfsdfdsf");
			}, (Exception e) => Console.WriteLine(e));

			p.Start();

			Console.ReadLine();
		}
	}
}
