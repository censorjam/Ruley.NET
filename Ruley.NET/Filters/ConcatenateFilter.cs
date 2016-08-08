using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class ConcatFilter : InlineFilter
    {
        [JsonProperty(Required = Required.Always)]
        List<Property<string>> Fields { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Property<string> Destination { get; set; }

        public override Event Apply(Event ev)
        {
            string value = string.Empty;
            foreach (var field in Fields)
            {
                value += ev.Data.GetValue(field.Get(ev)).ToString();
            }
            ev.Data.SetValue(Destination.Get(ev), value);
            return ev;
        }
    }
}