using System;
using System.Collections.Generic;
using System.Linq;
using Ruley.Core.Filters;

namespace Ruley.Core
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
        }

        public static void Register(Type filter)
        {
            KnownTypes.Add(filter);
        }

        public static object CreateProperty(Type pType, string value)
        {
            var ctor = pType.GetConstructor(new[] { typeof(object) });
            object instance = ctor.Invoke(new object[] { value });
            return instance;
        }

        public static Stage Resolve(dynamic d)
        {
            Stage filter = null;

            foreach (var VARIABLE in d)
            {
                var key = VARIABLE.Key;
                
                if (filter == null)
                {
                    var type = KnownTypes.First(t => t.Name.Replace("Stage", "").ToLower() == key.ToLower());
                    filter = (Stage)Activator.CreateInstance(type);
                    var primary = type.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PrimaryAttribute), false) != null);
                    var value = (string)VARIABLE.Value;
                    primary.SetValue(filter, CreateProperty(primary.PropertyType, value));
                }
                else
                {
                    var prop = filter.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == VARIABLE.Key.ToLower());

                    if (prop.PropertyType == typeof(Stage))
                    {
                        var value = Pipeline.FromDynamic(VARIABLE.Value);
                        prop.SetValue(filter, value);
                    }
                    else
                    {
                        var value = (string)VARIABLE.Value;
                        prop.SetValue(filter, CreateProperty(prop.PropertyType, value));
                    }
                }
            }
            return filter;
        }
    }
}