using System;

namespace Ruley.Core
{
    public class RuleParseException : Exception
    {
        public RuleParseException()
        {
        }

        public RuleParseException(string filename, string message, Exception inner) : base(message, inner)
        {
            FileName = filename;
        }

        public string FileName { get; set; }
    }
}