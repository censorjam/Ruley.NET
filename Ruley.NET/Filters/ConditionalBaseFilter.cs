using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public abstract class ConditionalBaseFilter : InlineFilter
    {
        [JsonProperty(Required = Required.Always)]
        public Property<double> Field { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Property<object> Level { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Property<string> Match { get; set; }

        protected bool RunMatch(Event m)
        {
            try
            {
                IDictionary<string, object> e = m.Data;
                
                Logger.Debug("casting value");
                var f = Field.Get(m);

                var value = Convert.ToDouble(f);

                //value = f;

                Logger.Debug("getting level");
                var level = Convert.ToDouble(Level.Get(m));

                Logger.Debug("value: {0}, level: {1}, match: {2}", value, level, Match);

                switch (Match.Get(m))
                {
                    case ">":
                        if (!(value > level))
                            return false;
                        break;

                    case ">=":
                        if (!(value >= level))
                            return false;
                        break;

                    case "<=":
                        if (!(value <= level))
                            return false;
                        break;

                    case "<":
                        if (!(value < level))
                            return false;
                        break;

                    case "=":
                        if (!(value == level))
                            return false;
                        break;

                    case "!=":
                        if (!(value != level))
                            return false;
                        break;
                }
            }
            catch (Exception e)
            {
               // m.AddError(e.Message);
                Logger.Error(e);
                throw;
            }
            return true;
        }
    }
}