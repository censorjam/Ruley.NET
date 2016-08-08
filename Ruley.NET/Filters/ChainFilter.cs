using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reactive.Linq;
using Ruley.Core.Outputs;

namespace Ruley.Core.Filters
{
    public class ChainFilter : Filter
    {
        public IEnumerable<Filter> Filters { get; set; }

        protected override IObservable<Event> Observable(IObservable<Event> source)
        {
            Logger = new Logger();//hack

            foreach (var filter in Filters)
            {
                if (filter.Enabled)
                {
                    filter.Logger =  new Logger();

                    var f = filter;
                    source = source.Do(m => Logger.Debug("Chain > Applying filter {0}", f.GetType()));
                    source = f.Extend(source);
                }
            }
            return source;
        }
    }
}