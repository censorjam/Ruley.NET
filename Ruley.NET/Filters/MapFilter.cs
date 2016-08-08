using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    public class Mapping
    {
        public object Key { get; set; }
        public DynamicDictionary Value { get; set; }
    }

    public class MapFilter : InlineFilter
    {
        [JsonProperty(Required = Required.Always)]
        public Property<string> Value { get; set; }

        public Property<bool> IgnoreCase { get; set; }
        public Property<bool> RegexMatch { get; set; }

        private DynamicDictionary _default { get; set; }

        [JsonProperty(Required = Required.Always)]
        public JObject Map { get; set; }

        public override void ValidateComposition()
        {
        }

        public override Event Apply(Event msg)
        {
            foreach (var mapping in Map)
            {
                //var s = mapping.Key == null ? "null" : mapping.Key.ToString();
                //var value = Value.GetValue(msg);
                //var valueString = value == null ? "null" :value.ToString();

                //if (valueString == s)
                //{
                //    msg.Data.Merge(mapping.Value);
                //    return msg;
                //}
            }

            //if (_default == null)
            //{
            //    //todo needs optimising?
            //    var d = Map.FirstOrDefault(i => (i.Key is string && (string) i.Key == "$default"));
            //    if (d != null) 
            //        _default = d.Value;
            //}

            //if (_default == null)
            //    return msg;

            //msg.Data.Merge(_default);
            return msg;
        }
    }

    public class TestFilter : InlineFilter
    {
        public Property<string> Default { get; set; }

        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}
