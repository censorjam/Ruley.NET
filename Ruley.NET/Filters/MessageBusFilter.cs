using Ruley.Core.Inputs;

namespace Ruley.Core.Filters
{
    public class MessageBusPublisherFilter : InlineFilter
    {
        public Property<string> Key { get; set; }

        public override Event Apply(Event msg)
        {
            RuleManager.MessageBus.Publish(Key.Get(msg), msg);
            return msg;
        }
    }

    public class MessageBusSubscriberFilter : Input
    {
        public string Key { get; set; }

        public override void Start()
        {
            RuleManager.MessageBus.Subscribe(Key, x => { OnNext(x); });
        }
    }
}
