using System;
using System.Reactive.Subjects;

namespace Ruley
{



    public abstract class Stage : Component, IObservable<Event>, IObserver<Event>
    {
        private Subject<Event> _subject = new Subject<Event>();

        public virtual void Start()
        {
            
        }

        public void PushNext(Event e)
        {
            Logger.Debug("Pushing next from {0}", this.GetType().Name);
            _subject.OnNext(e);
        }

        public IDisposable Subscribe(IObserver<Event> observer)
        {
            Logger.Debug("Subscribing to {0}", this.GetType().Name);
            return _subject.Subscribe(observer);
        }

        public virtual void OnNext(Event value)
        {
            PushNext(value);
        }

        public void OnError(Exception error)
        {
            Logger.Error(error);
            _subject.OnError(error);
        }

        public void OnCompleted()
        {
            Logger.Debug("completed");
            throw new NotImplementedException();
        }
    }
}