using System;
using System.Reactive.Subjects;

namespace Ruley.NET
{
    public abstract class Stage : Component, IObservable<Event>, IObserver<Event>
    {
        private bool _hadFirst;

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

        public virtual void OnFirst(Event e)
        {
        }

        protected void PushNext(Event e)
        {
            Logger.Debug("Pushing next");
            _subject.OnNext(e);
        }

        protected virtual void Process(Event e)
        {
        }

        public IDisposable Subscribe(IObserver<Event> observer)
        {
            Logger.Debug("Subscription received");
            return _subject.Subscribe(observer);
        }

        public void OnNext(Event e)
        {
            if (!_hadFirst)
            {
                OnFirst(e);
                _hadFirst = true;
            }
            Process(e);
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