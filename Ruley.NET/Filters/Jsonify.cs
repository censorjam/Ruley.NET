using System;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class JsonifyFilter : InlineFilter
    {
        [JsonProperty(Required = Required.Always)]
        public string Source { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Destination { get; set; }

        public override Event Apply(Event msg)
        {
            throw new NotImplementedException();
        }
    }
}
