using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ruley.NET.External;
using System;

namespace Ruley.NET
{
    public class ConsoleStage : InlineStage
    {
        private readonly IConsoleOutput _console;

        public ConsoleStage(IConsoleOutput console)
        {
            _console = console;
        }

        [Primary]
        public Property<string> Message { get; set; }

        public override Event Apply(Event x)
        {
            string msg = null;
            if (Message != null)
            {
                msg = Message.Get(x);
            }

            if (msg == null)
            {
                _console.WriteLine(JsonConvert.SerializeObject(x, Formatting.None));
            }
            else
            {
                _console.WriteLine(Message.Get(x));
            }
            return x;
        }
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
}