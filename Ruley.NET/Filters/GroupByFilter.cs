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

        private string _filterTemplate;

        private readonly Subject<Event> _subject = new Subject<Event>();
        protected override IObservable<Event> Observable(IObservable<Event> source)
        {
            if (Filters != null)
            {
                Filter = Filters.ToSingle();
            }

            //create a clone of the filter to be used as a template
            if (_filterTemplate == null)
            {
                _filterTemplate = JsonConvert.SerializeObject(new FilterSerializationWrapper() { Filter = Filter }, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            }
            
            source.GroupBy(m => Key.Get(m)).Subscribe(i =>
            {
                var subject = new Subject<Event>();

                var filter = JsonConvert.DeserializeObject<FilterSerializationWrapper>(_filterTemplate,
                    new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto}).Filter;

                filter.Extend(subject.AsObservable()).Subscribe(_subject);
                i.Subscribe(subject);
            });

            return _subject.AsObservable();
        }
    }
}
