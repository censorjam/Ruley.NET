using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ruley.NET;

namespace Ruley
{
    public class StageBuilder
    {
        static StageBuilder()
        {
            IoC.RegisterStage<DebounceStage>();
            IoC.RegisterStage<ConsoleStage>();
            IoC.RegisterStage<IntervalStage>();
            IoC.RegisterStage<CountStage>();
            IoC.RegisterStage<IfStage>();
            IoC.RegisterStage<WhereStage>();
            IoC.RegisterStage<PrevStage>();
            IoC.RegisterStage<MergeStage>();
            IoC.RegisterStage<DistinctStage>();
            IoC.RegisterStage<ScriptStage>();
            IoC.RegisterStage<GraphiteStage>();
            IoC.RegisterStage<EmailStage>();
            IoC.RegisterStage<RandomStage>();
            IoC.RegisterStage<TimestampStage>();
            IoC.RegisterStage<GroupByStage>();
            IoC.RegisterStage<MapStage>();
            IoC.RegisterStage<PerfCounterStage>();
            IoC.RegisterStage<TemplateStage>();
            IoC.RegisterStage<UdpOutStage>();
        }

        public static object CreateProperty(Context ctx, Type pType, string value)
        {
            var ctor = pType.GetConstructor(new[] { typeof(Context), typeof(object) });
            object instance = ctor.Invoke(new object[] { ctx, value });
            return instance;
        }

        public static Stage Resolve(Context context, Dictionary<object, object> d)
        {
            Stage filter = null;

            foreach (var kvp in d)
            {
                var key = (string)kvp.Key;

                PropertyInfo prop;

                if (filter == null)
                {
                    filter = IoC.ResolveStage(key);
                    filter.Context = context;
                    prop = filter.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PrimaryAttribute), true).Length > 0);

                    if (prop == null)
                        continue;
                }
                else
                {
                    prop = filter.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == key);
                }

                if (prop == null)
                    throw new Exception(string.Format("Could not find property '{0}' on type {1}", key, filter.DisplayName));

                if (prop.PropertyType == typeof(Stage))
                {
                    var value = YamlParser.FromDynamic(context, kvp.Value);
                    prop.SetValue(filter, value);
                }

                else if (prop.PropertyType == typeof(DynamicDictionary))
                {
                    var dd = new DynamicDictionary();

                    var o = kvp.Value as List<object>;
                    foreach (Dictionary<object, object> o1 in o)
                    {
                        var k = o1.Keys.First() as string;
                        var v = o1.Values.First();
                        //need to handle complex objects here
                        dd[k] = YamlParser.TryAsNumber(v as string);
                    }
                    prop.SetValue(filter, dd);
                }

                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Property<>))
                {
                    if (kvp.Value == null)
                    {
                        prop.SetValue(filter, null);
                    }
                    else
                    {
                        var value = (string)kvp.Value;
                        prop.SetValue(filter, CreateProperty(context, prop.PropertyType, value));
                    }
                }

                //todo: need to find a better way of dealing with this (this is for the debug property)
                else if (prop.PropertyType == typeof(Boolean))
                {
                    var value = Boolean.Parse((string)kvp.Value);
                    prop.SetValue(filter, value);
                }
            }
            return filter;
        }
    }
}