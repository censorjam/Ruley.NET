using System;

namespace Ruley.NET
{
    public interface ITimeProvider
    {
        DateTime GetNow();
    }

    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.UtcNow;
        }
    }

    public class DebounceStage : InlineStage
    {
		[Primary]
        public Property<TimeSpan> Period { get; set; }
        public Property<long> Allow { get; set; }
        private long _count;

        private DateTime? _lastPublished;
        private ITimeProvider _timeProvider;

        public DebounceStage(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public override void OnFirst(Event e)
        {
            if (Allow == null)
                Allow = 1L;
        }

        public override Event Apply(Event msg)
        {
            var now = _timeProvider.GetNow();
            if (_lastPublished == null || _count < Allow.Get(msg) || (now - _lastPublished >= Period.Get(msg))) {
                _lastPublished = now;
                _count++;
                return msg;
            }
            else
            {
                _count = 0;
                return null;
            }
        }
    }
}
