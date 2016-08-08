using System;
using System.Text.RegularExpressions;

namespace Ruley.Core.Filters
{
    internal class RegexFilter : InlineFilter
    {
        public string Expression { get; set; }
        public Property<string> Field { get; set; }
        public Property<string> Destination { get; set; }
        public Property<bool> AllowNonMatches { get; set; }
        private Regex _regex;

        public override Event Apply(Event msg)
        {
            if (_regex == null)
                _regex = new Regex(Expression, RegexOptions.IgnoreCase);

            var value = msg.Data.GetValue(Field.Get(msg));
            var valueStr = value as string;

            if (valueStr == null)
                throw new Exception("Unable to apply regex to non-string value");

            MatchCollection matches = _regex.Matches(valueStr);
            var match = matches.Count > 0;

            if (Destination != null)
            {
                msg.Data.SetValue(Destination.Get(msg), match);
            }

            if (AllowNonMatches || match)
                return msg;

            return null;
        }
    }
}
