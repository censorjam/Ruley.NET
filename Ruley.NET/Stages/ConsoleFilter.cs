namespace Ruley
{
    public abstract class InlineStage : Stage
    {
        public override void Next(Event e)
        {
            var nx = Apply(e);

            if (nx != null)
                OnNext(nx);
        }

        public abstract Event Apply(Event x);
    }
}