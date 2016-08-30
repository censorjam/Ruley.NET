using Newtonsoft.Json;
using Ruley.NET.External;

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
                _console.WriteLine(JsonConvert.SerializeObject(x, Formatting.Indented));
            }
            else
            {
                _console.WriteLine(Message.Get(x));
            }
            return x;
        }
    }
}