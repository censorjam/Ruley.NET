using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class GroupByFilter : Filter
    {
        public Property<string> Key { get; set; }
        public Filter Filter { get; set; }
        public List<Filter> Filters { get; set; }

        private readonly Subject<Event> _subject = new Subject<Event>();
        protected override IObservable<Event> Observable(IObservable<Event> source)
        {
            if (Filters != null)
            {
                Filter = Filters.ToSingle();
            }

            source.GroupBy(m => Key.GetValue(m)).Subscribe(i =>
            {
                var subject = new Subject<Event>();
                var serialize = JsonConvert.SerializeObject(new FilterSerializationWrapper() { Filter = Filter }, new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto});

                var filter = JsonConvert.DeserializeObject<FilterSerializationWrapper>(serialize,
                    new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto}).Filter;

                filter.Extend(subject.AsObservable()).Subscribe(_subject);
                i.Subscribe(subject);
            });

            return _subject.AsObservable();
        }
    }
}
