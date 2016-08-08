using System.Dynamic;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Ruley.Core.Filters
{
    public class CountFilter : InlineFilter
    {
        private long _count;
        
        [JsonProperty(Required = Required.Always)]
        public Property<string> Field { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Property<long> Period { get; set; }

        private List<DateTime> _items = new List<DateTime>();

        public override Event Apply(Event msg)
        {
            var now = DateTime.UtcNow;
            _items.Add((DateTime)msg.Data["$created"]);

            while(_items.Count > 0 && _items[0] <= (now - TimeSpan.FromMilliseconds(Period.Get(msg))))
            {
                _items.RemoveAt(0);
            }

            msg.Data.SetValue(Field.Get(msg), _items.Count);
            return msg;
        }
    }

    //public class CountFilter : InlineFilter
    //{
    //    private long _count;

    //    [JsonProperty(Required = Required.Always)]
    //    public Property<string> Field { get; set; }

    //    public override Event Apply(Event msg)
    //    {
    //        _count++;
    //        var destination = Field.Get(msg);
    //        msg.Data.SetValue(destination, _count);
    //        return msg;
    //    }
    //}
}
