using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class DistinctFilter : InlineFilter
    {
        public Property<string> Value { get; set; }

        private bool _hadValue;
        private string _prevValue;

        public override Event Apply(Event msg)
        {
            object n;
            n = Value.GetValue(msg);

            if (n != null)
            {
                var next = JsonConvert.SerializeObject(n);
                if (!next.Equals(_prevValue) || !_hadValue)
                {
                    _hadValue = true;
                    _prevValue = next;
                    return msg;
                }
                
                return null;
            }
            else
            {
                if (_prevValue != null || !_hadValue)
                {
                    _hadValue = true;
                    _prevValue = null;
                    return msg;
                }
                return null;
            }
        }
    }
}
