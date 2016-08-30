using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Ruley.NET
{
    public class ThrottleFilter : Filter
    {
        [JsonProperty(Required = Required.Always)]
        public double Interval { get; set; }
        public string Key { get; set; }
        public string CountField { get; set; }
        public bool AllowEmpty { get; set; }

        private readonly Subject<Event> _subject = new Subject<Event>();

        protected override IObservable<Event> Observable(IObservable<Event> source)
        {
            source.Buffer(TimeSpan.FromMilliseconds(Interval)).Subscribe(x =>
            {
                if (AllowEmpty || x.Count > 0)
                    _subject.OnNext(Reduce(x));
            });

            return _subject.AsObservable();
        }

        private Event Reduce(IList<Event> msgs)
        {
            var count = msgs.Count;
            if (count == 0)
            {
                //todo massive bug creating an event here, needs to use context!
                msgs = new List<Event> { Context.GetNext() };
            }

            msgs[0].SetValue(CountField ?? "count", count);
            return msgs[0];
        }
    }
}