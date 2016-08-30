using NUnit.Framework;

namespace Ruley.NET
{
    public class SkipFilter : InlineFilter 
    {
        public Property<long> Count { get; set; }

        private long _current;

        public override Event Apply(Event msg)
        {
            if (_current < Count.Get(msg))
            {
                _current++;
                return null;
            }
            return msg;
        }
    }

    [TestFixture]
    public class SkipTests
    {
        
    }
}
