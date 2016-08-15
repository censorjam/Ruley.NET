using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ruley.Core.Filters;

namespace Ruley
{
    public class StageBuilder
    {
        private static List<Type> KnownTypes = new List<Type>();

        static StageBuilder()
        {
            Register(typeof(ConsoleStage));
            Register(typeof(IntervalStage));
            Register(typeof(CountStage));
            Register(typeof(IfStage));
            Register(typeof(WhereStage));
            Register(typeof(PrevStage));
            Register(typeof(DebounceStage));
            Register(typeof(MergeStage));
			Register(typeof(DistinctStage));
            Register(typeof(ScriptStage));
            Register(typeof(GraphiteStage));
        }

		public static void Register(Type filter)
        {
            KnownTypes.Add(filter);
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
                    var type = KnownTypes.First(t => t.Name.Replace("Stage", "").ToLower() == key.ToLower());
                    filter = (Stage)Activator.CreateInstance(type);
	                filter.Ctx = context;
					prop = type.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PrimaryAttribute), true).Length > 0);

	                if (prop == null)
		                continue;
                }
                else
                {
                    prop = filter.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == key);
                }

                if (prop.PropertyType == typeof(Stage))
                {
                    var value = YamlParser.FromDynamic(context, kvp.Value);
                    prop.SetValue(filter, value);
                }
                else if (prop.PropertyType == typeof(Event))
                {
                    var dd = new Event();

                    var o = kvp.Value as List<object>;
                    foreach (Dictionary<object, object> o1 in o)
                    {
                        var k = o1.Keys.First() as string;
                        var v = o1.Values.First();
                        dd[k] = v;
                    }
                    prop.SetValue(filter, dd);
                }
                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Property<>))
                {
                    var value = (string)kvp.Value;
                    prop.SetValue(filter, CreateProperty(context, prop.PropertyType, value));
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