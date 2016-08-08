using System;
using System.Dynamic;
using SmartFormat;

namespace Ruley.Core.Outputs
{
    public class EmailOutput : Output
    {
        public string To { get; set; }
        public Property<string> Subject { get; set; }
        public Property<string> Body { get; set; }

        public override void Do(Event x)
        {
            var email = "--------------" + 
                Environment.NewLine + "To: " + To + 
                Environment.NewLine + "Subject: " + 
                Subject.Get(x) + Environment.NewLine + 
                "Body: " + Body.Get(x);

            Logger.Debug(email);
        }
    }
}