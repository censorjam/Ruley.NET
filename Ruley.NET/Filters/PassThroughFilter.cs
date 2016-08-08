namespace Ruley.Core.Filters
{
    public class PassThroughFilter : InlineFilter
    {
        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}