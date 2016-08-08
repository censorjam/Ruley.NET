using SmartFormat;

namespace Ruley.Core
{
    public static class Templater
    {
        public static string ApplyTemplate(string template, object o)
        {
            return Smart.Format(template, o);
        }
    }
}