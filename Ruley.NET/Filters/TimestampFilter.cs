using System;
using Newtonsoft.Json;

namespace Ruley.Core.Filters
{
    public class TimestampFilter : InlineFilter
    {
        [JsonRequired]
        public string Destination { get; set; }

        public override Event Apply(Event msg)
        {
            msg.Data[Destination] = DateTime.UtcNow;
            return msg;
        }
    }
}
