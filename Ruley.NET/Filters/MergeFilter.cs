using Newtonsoft.Json;
using Ruley.Core.Outputs;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    public class MergeFilter : InlineFilter
    {
        [JsonRequired]
        public DynamicDictionary Value { get; set; }
        
        public override Event Apply(Event e)
        {
            foreach (var v in Value)
            {
                var str = v.Value as string;
                if (str != null)
                {
                    //todo cache this
                    var g = new TemplatedPropertyGetter(str);
                    e.Data[v.Key] = g.GetValue(str, e);
                }
                else
                {
                    e.Data[v.Key] = v.Value;
                }
            }
            return e;
        }
    }
}
