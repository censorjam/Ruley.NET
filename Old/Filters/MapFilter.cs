using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ruley.NET
{
    public class Mapping
    {
        public object Key { get; set; }
        public Event Value { get; set; }
    }

   

    public class TestFilter : InlineFilter
    {
        public Property<string> Default { get; set; }

        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}
