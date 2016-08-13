using System;
using System.Threading;
using Newtonsoft.Json;

namespace Ruley.Core.Inputs
{
    public class IntervalInput : Input
    {
        [JsonProperty(Required = Required.Always)]
        public int Period { get; set; }

        private Timer _timer;

        public override void Start()
        {
            _timer = new Timer(state =>
            {
                OnTick();
            });
            _timer.Change(0, Period);
        }

        public override void Dispose()
        {
            _timer.Dispose();
        }

        public override void ValidateComposition()
        {
            if (Period < 0)
                throw new Exception("Interval cannot be negative");
        }

        public virtual void OnTick()
        {
            OnNext(new DynamicDictionary());
        }
    }
}