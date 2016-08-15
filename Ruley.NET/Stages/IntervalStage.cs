using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Ruley
{
    public class IntervalStage : Stage
    {
        [Primary]
        public Property<TimeSpan> Period { get; set; }

        public override void Start()
        {
            //var p = Period.Get(null);
            //Observable.Timer(TimeSpan.Zero, p).Subscribe(l =>
            //{
            //    Logger.Debug("Tick");

            //    //while (true)
            //    OnNext(new Event());
            //});

            Task.Run(() =>
            {
                while (true)
                    OnNext(new Event());
            });
        }

        public override void OnNext(Event e)
        {
            PushNext(e);
        }
    }
}