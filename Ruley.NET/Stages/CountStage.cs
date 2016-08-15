using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Ruley
{
    public class CountStage : InlineStage
    {
        private long _count;
        
        [JsonProperty(Required = Required.Always)]
		[Primary]
        public Property<string> Field { get; set; }

        private DateTime? _lastReset;
		public Property<TimeSpan> Period { get; set; }

        public Property<bool> Where { get; set; }
        private List<DateTime> _items = new List<DateTime>();

        public override Event Apply(Event msg)
        {
            if (_lastReset == null)
                _lastReset = DateTime.UtcNow;

            if (Period != null)
            {
                var now = DateTime.UtcNow;

                if (now - _lastReset < Period.Get(msg))
                {
                    _count++;
                }
                else
                {
                    _count = 1;
                    _lastReset = now;
                }
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
            }

            msg.SetValue(Field.Get(msg), _count);
            return msg;
        }
    }
}
