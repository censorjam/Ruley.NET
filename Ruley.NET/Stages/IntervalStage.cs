using System;
using System.Threading;
using Ruley;
using System.Reactive.Linq;

namespace Ruley
{
    public class IntervalStage : Stage
    {
        public Property<TimeSpan> Period { get; set; }

        public override void Start()
        {
            var p = Period.Get(null);
            Observable.Timer(TimeSpan.Zero, p).Subscribe(l =>
            {
                Logger.Debug("Tick");
                OnNext(new Event());
            });
        }

        public override void OnNext(Event e)
        {
            PushNext(e);
        }
    }
}