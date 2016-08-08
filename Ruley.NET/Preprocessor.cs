using System.Collections.Generic;

namespace Ruley.Core
{
    public class Preprocessor
    {
        private Dictionary<string, string> _aliases = new Dictionary<string, string>();

        public void Alias(string alias, string value)
        {
            _aliases[alias] = value;
        }

        public string Process(string rule)
        {
            foreach (var alias in _aliases)
            {
                rule = rule.Replace("\"" + alias.Key + "\"", "\"" + alias.Value + "\"");
            }
            return rule;
        }
    }
}
