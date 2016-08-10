using System;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class BranchFilter : InlineFilter
    {
        [JsonRequired]
        public Property<bool> Value { get; set; }
        public InlineFilter Then { get; set; }
        public InlineFilter Else { get; set; }

        public override Event Apply(Event x)
        {
            Then = Then ?? new PassThroughFilter();
            Else = Else ?? new BlockFilter();
            var match = Value.Get(x);
            var next = match ? DoTrue(x) : DoFalse(x);
            return next;
        }

        private Event DoFalse(Event e)
        {
            Logger.Debug("Executing false branch value");
            return Else.Apply(e);
        }

        private Event DoTrue(Event e)
        {
            Logger.Debug("Executing true branch");
            return Then.Apply(e);
        }
    }
}