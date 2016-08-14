using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Ruley
{
	public class YamlFile
	{
		public Dictionary<object, object>[] Parameters { get; set; }
		public object Definition { get; set; }
	}

	public class YamlParser
	{
		public static Pipeline Load(string file)
		{
			var s = File.ReadAllText(file);
			var input = new StringReader(s);

			var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
			var y = deserializer.Deserialize<YamlFile>(input);
			var z = YamlParser.ToImmutable(y.Parameters[0]);
			var p = YamlParser.FromDynamic(new Context(), y.Definition);

			return p;
		}

		public static Pipeline FromDynamic(Context ctx, dynamic def)
		{
			var chain = new Pipeline();
			chain.Ctx = ctx;

			Stage prev = null;
			foreach (var f in def)
			{
				Stage stage = StageBuilder.Resolve(ctx, f);
				chain.Stages.Add(stage);

				if (prev != null)
				{
					prev.Subscribe(stage);
				}

				prev = stage;
			}
			return chain;
		}

		public static ImmutableDictionary<string, object> ToImmutable(IDictionary<object, object> source)
		{
			var o = ImmutableDictionary<string, object>.Empty;

			foreach (var o1 in source)
			{
				if (o1.Value is IDictionary<object, object>)
				{
					o = o.SetItem((string)o1.Key, ToImmutable((IDictionary<object, object>)o1.Value));
				}
				else
				{
					double d;
					if (double.TryParse((string)o1.Value, out d))
					{
						o = o.SetItem((string)o1.Key, d);
					}
					else
					{
						o = o.SetItem((string)o1.Key, o1.Value);
					}
				}
			}

			return o;
		}
	}
}