using Ruley.Core.Filters;
using System;
using Ruley.Core;
using Graphite;
using Ruley.NET.Logging;

namespace Ruley.NET.Filters
{
    public class GraphiteOutputFilter : InlineFilter
    {
        public Property<double?> Value { get; set; }
        public Property<string> Metric { get; set; }
        public Property<string> Host { get; set; }
        public Property<long> Port { get; set; }

        public override Event Apply(Event msg)
        {
            using (var client = new GraphiteUdpClient(Host.Get(msg), (int)Port.Get(msg)))
            {
                var value = Value.Get(msg);
                if (value != null)
                {
                    if (value < int.MaxValue)
                    {
                        Logger.Debug("Sending to graphite {0} : {1}", Metric.Get(msg), Convert.ToInt32(value));
                        client.Send(Metric.Get(msg), Convert.ToInt32(value));
                    }
                }
            }
            return msg;
        }
    }
}
