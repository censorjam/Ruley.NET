using System;

namespace Ruley.NET
{
    public class DebounceStage : InlineStage
    {
        [Primary]
        public Property<TimeSpan> Period { get; set; }
        public Property<long> Allow { get; set; }

        private long _count;
        private DateTime? _lastPublished;
        public ITimeProvider TimeProvider { get; set; }

        public DebounceStage(ITimeProvider timeProvider)
        {
            TimeProvider = timeProvider;
        }

        public override void OnFirst(Event e)
        {
            if (Allow == null)
                Allow = 1L;
        }

        public override Event Apply(Event msg)
        {
            var now = TimeProvider.GetNow();

            if (now - _lastPublished >= Period.Get(msg))
                _count = 0;

            if (_lastPublished == null || _count < Allow.Get(msg) || (now - _lastPublished >= Period.Get(msg)))
            {
                _lastPublished = now;
                _count++;
                return msg;
            }
            else
            {
                return null;
            }
        }
    }
}
