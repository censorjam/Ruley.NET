using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace Ruley.Core.Outputs
{
    public class ConsoleOutput : Output
    {
        public Property<string> Message { get; set; }

        public override void Do(Event x)
        {
            if (Message != null)
            {
                Console.WriteLine(Message.Get(x));
            }
            else
            {
                Console.WriteLine(JsonConvert.SerializeObject(x, Formatting.Indented));
            }
        }
    }
}