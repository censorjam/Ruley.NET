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
            string msg = null;
            if (Message != null)
            {
                msg = Message.Get(x);
            }

            if (msg == null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(x, Formatting.Indented));
            }
            else
            {
                Console.WriteLine(Message.Get(x));
            }
            return x;
        }
    }
}