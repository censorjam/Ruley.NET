using Ruley.NET.Logging;
using System;
using System.Reactive.Subjects;

namespace Ruley
{
    public abstract class Stage : Component, IObservable<Event>, IObserver<Event>
    {
        public Stage()
        {

        }

        private string _displayName;
        public string DisplayName
        {
            get
            {
                if (_displayName == null)
                    _displayName = GetType().Name.Substring(0, GetType().Name.Length - 5);

                return _displayName;
            }
        }

        private Subject<Event> _subject = new Subject<Event>();

        public virtual void Start()
        {
        }

        public void PushNext(Event e)
        {
            Logger.Debug("Pushing next");
            _subject.OnNext(e);
        }

        public IDisposable Subscribe(IObserver<Event> observer)
        {
            Logger.Debug("Subscription received");
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