using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Ruley.NET
{
    public abstract class InlineFilter : Filter
    {
        public abstract Event Apply(Event msg);

        private readonly Subject<Event> _subject = new Subject<Event>();
        protected override IObservable<Event> Observable(IObservable<Event> source)
        {
            source.Subscribe(m =>
            {
                var next = Apply(m);
                if (next != null)
                {
                    _subject.OnNext(next);
                }
            });
            return _subject.AsObservable();
        }
    }
}