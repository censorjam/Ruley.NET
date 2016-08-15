using Newtonsoft.Json;
using Ruley.Core.Outputs;

namespace Ruley.Core.Filters
{
    public class MergeStage : InlineStage
    {
		//needs to filter out $ fields!
        [JsonRequired]
        [Primary]
        public Event Value { get; set; }
        
        public override Event Apply(Event e)
        {
            foreach (var v in Value)
            {
                var str = v.Value as string;
                if (str != null)
                {
                    //todo cache this
                    var g = new TemplatedPropertyGetter(Ctx, str);
                    e[v.Key] = g.GetValue(str, e);
                }
                else
                {
                    e[v.Key] = v.Value;
                }
            }
            return e;
        }
    }
}
