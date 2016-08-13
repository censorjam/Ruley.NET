using System.Dynamic;
using Newtonsoft.Json;

namespace Ruley
{
    public class PrevStage : InlineStage
    {
        [Primary]
        public Property<string> Field { get; set; }

        private DynamicDictionary _prev;

        public override Event Apply(Event msg)
        {
            msg.Data.SetValue(Field.Get(msg), _prev);
            _prev = msg.Data.Clone();
            _prev.Remove(Field.Get(msg));
            return msg;
        }
    }
}