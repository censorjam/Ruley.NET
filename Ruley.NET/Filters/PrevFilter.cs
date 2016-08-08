using System.Dynamic;
using Newtonsoft.Json;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    public class PrevFilter : InlineFilter
    {
        [JsonProperty(Required = Required.Always)]
        public string Destination { get; set; }

        private DynamicDictionary _prev;

        public override Event Apply(Event msg)
        {
            msg.Data.SetValue(Destination, _prev);
            _prev = msg.Data.Clone();
            _prev.Remove(Destination);
            return msg;
        }
    }
}