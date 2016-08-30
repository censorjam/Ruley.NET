namespace Ruley.NET
{
    public class PrevStage : InlineStage
    {
        [Primary]
        public Property<string> Field { get; set; }

        private Event _prev;

        public override Event Apply(Event msg)
        {
            msg.SetValue(Field.Get(msg), _prev);
            _prev = msg.Clone();
            _prev.Remove(Field.Get(msg));
            return msg;
        }
    }
}