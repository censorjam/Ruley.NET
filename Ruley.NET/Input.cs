using System;
using System.Dynamic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ruley.Core.Outputs;
using Ruley.Dynamic;

namespace Ruley.Core.Inputs
{
    public abstract class Input : Component
    {
        public abstract void Start();
        private readonly Subject<Event> _subject = new Subject<Event>();
        private readonly object _lock = new object();
        public ExpandoObject Properties { get; set; }

        public IObservable<Event> Source
        {
            get { return _subject.AsObservable(); }
        }

        public void OnNext(DynamicDictionary next)
        {
            var ev = Context.GetNext();
            ev.Data.Merge(next);
            lock (_lock)
            {
                _subject.OnNext(ev);
            }
        }

        public ExpandoObject ToExpando(object obj)
        {
            var converter = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(obj), converter);
        }

        public ExpandoObject FromJson(string json)
        {
            var converter = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<ExpandoObject>(json, converter);
        }
    }

    public class MessageBusInput : Input
    {
        public string Key { get; set; }

        public override void Start()
        {
            RuleManager.MessageBus.Subscribe(Key, e => { OnNext(e.Data); });
        }
    }
}