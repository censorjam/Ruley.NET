using System.Collections.Generic;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    public class ReplayFieldFilter : InlineFilter
    {
        public string Field { get; set; }

        private DynamicDictionary _prev;

        public override Event Apply(Event msg)
        {
            var m = (IDictionary<string, object>)msg.Data;
            if (!msg.Data.HasProperty(Field) && _prev != null)
            {
                m[Field] = _prev.GetValue(Field);
            }
            _prev = msg.Data;
            return msg;
        }
    }
}