using System;
using System.Reactive.Linq;

namespace Ruley.NET
{
    public class IntervalStage : Stage
    {
        [Primary]
        public Property<TimeSpan> Period { get; set; }

        public override void Start()
        {
            var p = Period.Get(null);
            Observable.Timer(TimeSpan.Zero, p).Subscribe(l =>
            {
                Logger.Debug("Tick");

                //while (true)
                PushNext(Context.GetNext());
            });

            //Task.Run(() =>
            //{
            //    for(var i = 0; i < 10000000; i++)
            //        PushNext(new Event());

            //    Logger.Info("Stopped");
            //});
        }
    }
}