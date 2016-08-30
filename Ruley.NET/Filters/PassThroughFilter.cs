namespace Ruley.NET
{
    public class PassThroughFilter : InlineFilter
    {
        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}