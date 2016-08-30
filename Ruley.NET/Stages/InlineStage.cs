namespace Ruley.NET
{
    public abstract class InlineStage : Stage
	{
		protected override void Process(Event e)
		{
		    var nx = Apply(e);

			if (nx != null)
				PushNext(nx);
		}

		public abstract Event Apply(Event x);
	}
}