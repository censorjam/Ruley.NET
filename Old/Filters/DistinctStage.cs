namespace Ruley.NET
{
	public class DistinctStage : InlineStage
	{
		private Event _prev;

		public override Event Apply(Event msg)
		{
			var equal = msg.Equals(_prev);
			_prev = msg.Clone();

			if (!equal)
			{
				return msg;
			}
			else
			{
				return null;
			}
		}
	}
}
