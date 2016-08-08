using System.Dynamic;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class ConditionalFilter : ConditionalBaseFilter
    {
        [JsonProperty(Required = Required.Always)]
        public Property<string> Destination { get; set; }

        public override Event Apply(Event ev)
        {
            var match = RunMatch(ev);
            ev.Data.SetValue(Destination.Get(ev), match);
            return ev;
        }
    }
}