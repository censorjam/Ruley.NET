using System;
using Newtonsoft.Json;

namespace Ruley
{
    public class ConsoleStage : InlineStage
    {
        [Primary]
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