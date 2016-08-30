using System;

namespace Ruley.NET
{
    public class RandomStage : InlineStage
    {
        [Primary]
        public Property<string> Destination { get; set; }

        private Random _r = new Random();

        public override Event Apply(Event x)
        {
            x[Destination.Get(x)] = _r.Next();
            return x;
        }
    }
}