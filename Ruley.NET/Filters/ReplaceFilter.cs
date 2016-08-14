using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruley.Core.Filters.Experimental
{
    class ReplaceFilter : InlineFilter
    {
        public Property<Event> Data { get; set; } 

        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}
