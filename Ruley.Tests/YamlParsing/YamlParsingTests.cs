using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.IO;
using System.Dynamic;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;
using System.Runtime.CompilerServices;

namespace Ruley.Tests
{
	[TestFixture]
	public class YamlParsingTests
	{
		private string LoadFromResource()
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("Ruley.Tests.YamlParsing.Map1.yaml"))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		[Test]
		public void Test1()
		{
			var yaml = LoadFromResource();
			var x = YamlParser.Load(yaml);

			Event last = null;
			x.Subscribe(s => { last = s; });
			x.Start();

			dynamic next = new Event(new DynamicDictionary());
			next.a = "abc";
			x.OnNext(next);

			Assert.AreEqual("XYZ", last["newfield"]);
		}






		[Test]
		public void Templater()
		{
			var template = "hello {x} {yo.t}";

			var t = new Templater();
			t.Compile(template);
			//var data = new { x = "world", yo = new { t = "bang" } };

			dynamic data = new Event();
			data.x = "world";
			data.yo = new { t = "bang" };

			var s = t.Template(data);
		}
	}

}

