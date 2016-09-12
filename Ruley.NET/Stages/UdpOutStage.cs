using Ruley.NET.External;

namespace Ruley.NET
{
    public class UdpOutStage : InlineStage
    {
        [Primary]
        public Property<string> Message { get; set; }
        public Property<string> Host { get; set; }
        public Property<int> Port { get; set; }

        private IUdpOutClient _client;

        public UdpOutStage(IUdpOutClient client)
        {
            _client = client;
        }

        public override Event Apply(Event x)
        {
            _client.Send(Host.Get(x), Port.Get(x), Message.Get(x));
            return x;
        }
    }
}
