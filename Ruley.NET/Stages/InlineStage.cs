using System;

namespace Ruley
{
    public abstract class InlineStage : Stage
    {
        public override void OnNext(Event e)
        {
            try
            {
                var nx = Apply(e);

                if (nx != null)
                    PushNext(nx);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public abstract Event Apply(Event x);
    }
}