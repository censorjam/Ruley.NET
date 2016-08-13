using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Ruley
{
    public class CountStage : InlineStage
    {
        private long _count;
        
        [JsonProperty(Required = Required.Always)]
        public Property<string> Field { get; set; }
        public Property<TimeSpan> Period { get; set; }
        public Property<bool> Where { get; set; }

        private List<DateTime> _items = new List<DateTime>();

        public override Event Apply(Event msg)
        {
            if (Period != null)
            {
                var now = DateTime.UtcNow;

                if (Where == null || Where.Get(msg))
                {
                    _items.Add((DateTime)msg.Data["$created"]);
                }
                else
                {
                    _items.Clear();
                }

                while (_items.Count > 0 && _items[0] <= (now - Period.Get(msg)))
                {
                    _items.RemoveAt(0);
                }

                msg.Data.SetValue(Field.Get(msg), _items.Count);
                return msg;
            }
            else
            {
                if (Where == null || Where.Get(msg))
                {
                    _count++;
                }
                else
                {
                    _count = 0;
                }

                var destination = Field.Get(msg);
                msg.Data.SetValue(destination, _count);
                return msg;
            }
        }
    }
}
