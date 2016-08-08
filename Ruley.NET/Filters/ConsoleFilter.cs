using System;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class ConsoleFilter : InlineFilter
    {
        public Property<string> Message { get; set; }

        public override Event Apply(Event x)
        {
            if (Message != null)
            {
                Console.WriteLine(Message.Get(x));
            }
            else
            {
                Console.WriteLine(JsonConvert.SerializeObject(x.Data, Formatting.Indented));
            }
            return x;
        }
    }
}