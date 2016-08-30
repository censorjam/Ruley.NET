using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ruley.NET
{
    public class Mapping
    {
        public object Key { get; set; }
        public Event Value { get; set; }
    }

    public class MapStage : InlineStage
    {
        [Primary]
        public Property<string> Value { get; set; }
		public Property<string> To { get; set; }
		public DynamicDictionary With { get; set; }

		public Property<bool> IgnoreCase { get; set; }
        public Property<bool> RegexMatch { get; set; }

        private Event _default { get; set; }

        [JsonProperty(Required = Required.Always)]
        public JObject Map { get; set; }

        public override void ValidateComposition()
        {
        }

        public override Event Apply(Event msg)
        {
	        var value = Value.Get(msg);
	        object o;
	        With.TryGetValue(value, out o);

	        if (o == null)
	        {
				throw new Exception(String.Format("Value not present in mapping '{0}'", value));
	        }

	        msg[To.Get(msg)] = With[value];
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
