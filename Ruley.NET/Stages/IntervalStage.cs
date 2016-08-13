using System;
using System.Threading;
using Ruley;

namespace Ruley
{
    public class IntervalStage : Stage
    {
        public Property<TimeSpan> Period { get; set; }
        private Timer _timer;

        public override void Start()
        {
            _timer = new Timer(state =>
            {
                Logger.Debug("Tick");
                OnNext(new Event(new DynamicDictionary()));
            });
            _timer.Change(0, (int)Period.Get(null).TotalMilliseconds);
        }

        public override void Next(Event e)
        {
            OnNext(e);
        }
    }
}