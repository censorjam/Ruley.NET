using System;

namespace Ruley
{
    public class DebounceStage : InlineStage
    {
		[Primary]
        public Property<TimeSpan> Period { get; set; }

        private DateTime? _lastPublished;

        public override Event Apply(Event msg)
        {
            var now = DateTime.UtcNow;
            if (_lastPublished == null || (now - _lastPublished > Period.Get(msg))) {
                _lastPublished = now; 
                return msg;
            }
            else
            {
                return null;
            }
        }
    }
}
