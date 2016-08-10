using Ruley.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruley.Core;

namespace Ruley.NET.Filters
{
    public class DebounceFilter : InlineFilter
    {
        public DebounceFilter()
        {

        }

        public Property<long> Period { get; set; }

        private DateTime? _lastPublished;

        public override Event Apply(Event msg)
        {
            var now = DateTime.UtcNow;
            if (_lastPublished == null || (now - _lastPublished > TimeSpan.FromMilliseconds(Period.Get(msg)))) {
                _lastPublished = now; 
                return msg;
            }
            else
            {
                return null;
            }
        }
    }
}
