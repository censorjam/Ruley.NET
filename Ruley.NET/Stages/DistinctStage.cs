namespace Ruley.NET
{
    public class DistinctStage : InlineStage
    {
        [Primary]
        public Property<string> Value { get; set; }
        private string _prevValue;

        public override Event Apply(Event msg)
        {
            var value = Value.Get(msg);
            if (!value.Equals(_prevValue))
            {
                _prevValue = value;
                return msg;
            }
            else
            {
                _prevValue = value;
                return null;
            }
        }
    }
}
