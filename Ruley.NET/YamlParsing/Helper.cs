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
        private static Logger Logger = new Logger("YamlParser", false);

		public static Pipeline Load(string file)
		{
            Logger.Info("Loading file {0}", file);
            var s = File.ReadAllText(file);
            Logger.Info(s);
            var input = new StringReader(s);

            Logger.Info("Deserializing...");
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
			var y = deserializer.Deserialize<YamlFile>(input);

            Logger.Debug("Parameters");

            var parameters = new ImmutableDictionary<string, object>[y.Parameters.Length];
            for(var i = 0; i < y.Parameters.Length; i++)
            {
                parameters[i] = YamlParser.ToImmutable(y.Parameters[i]);
            }

			var p = YamlParser.FromDynamic(new Context() { Parameters = new DynamicDictionary(parameters[0]) }, y.Definition);
            Logger.Info("Loaded successfully");

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
                Logger.Info("{0} loaded", stage.DisplayName);
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