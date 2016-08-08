using System.Collections.Generic;

namespace Ruley.Core.Filters
{
    public static class FilterExtensions
    {
        public static Filter ToSingle(this IEnumerable<Filter> filters)
        {
            var chain = new ChainFilter {Filters = filters};
            return chain;
        }
    }
}