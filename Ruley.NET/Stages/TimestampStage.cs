using System;

namespace Ruley.NET
{
    public class TimestampStage : InlineStage
    {
        [Primary]
        public Property<string> Field { get; set; }

        public override Event Apply(Event x)
        {
            x[Field.Get(x)] = DateTime.UtcNow;
            return x;
        }
    }
}