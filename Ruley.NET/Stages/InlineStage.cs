using System;

namespace Ruley
{
	public abstract class InlineStage : Stage
	{
		public override void OnNext(Event e)
		{
			var nx = Apply(e);

			if (nx != null)
				PushNext(nx);
		}

		public abstract Event Apply(Event x);
	}
}