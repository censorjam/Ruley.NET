using Mustache;
using SmartFormat;

namespace Ruley.Core
{
    public static class Templater
    {
        public static string ApplyTemplate(string template, object o)
        {
            FormatCompiler compiler = new FormatCompiler();
            Generator generator = compiler.Compile(template);
            string result = generator.Render(o);

            return template;
            //return result;
            //return Smart.Format(template, o);
        }
    }
}