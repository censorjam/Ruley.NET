using System;
using System.Reactive.Linq;

namespace Ruley.NET
{
    public abstract class Filter : Component
    {
        public IObservable<Event> Extend(IObservable<Event> source)
        {
            return Observable(source.Do(m =>
            {
                Logger.Debug("Entering {0}", GetType().Name);
            }));
        }

        protected abstract IObservable<Event> Observable(IObservable<Event> source);
    }
}