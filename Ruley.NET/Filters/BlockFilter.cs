using System.Dynamic;

namespace Ruley.Core.Filters
{
    public class BlockFilter : InlineFilter
    {
        public override Event Apply(Event msg)
        {
            return null;
        }
    }
}