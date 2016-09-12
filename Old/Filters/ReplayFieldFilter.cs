using System.Collections.Generic;

namespace Ruley.NET
{
    public class ReplayFieldFilter : InlineFilter
    {
        public string Field { get; set; }

        private Event _prev;

        public override Event Apply(Event msg)
        {
            var m = (IDictionary<string, object>)msg;
            if (!msg.HasProperty(Field) && _prev != null)
            {
                m[Field] = _prev.GetValue(Field);
            }
            _prev = msg;
            return msg;
        }
    }
}