using SmartFormat;
using Ruley.Core.Outputs;

namespace Ruley.Core
{
    public static class Templater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ApplyTemplate(string template, Event obj)
        {
			//FormatCompiler compiler = new FormatCompiler();
			//Generator generator = compiler.Compile(template);
			//string result = generator.Render(o);

			//return template;
			////return result;
			return Smart.Format(template, obj);

			//var prev = ' ';
   //         int start = 0;
   //         var inT = false;

   //         StringBuilder o = new StringBuilder();

   //         for (var i = 0; i < template.Length; i++)
   //         {
   //             if (template[i] == '{' && prev == '{')
   //             {
   //                 start = i;
   //                 inT = true;
   //             }
   //             else if (template[i] == '}' && prev == '}')
   //             {
   //                 var e = template.Substring(start + 1, i - start - 2);
   //                 var s = e.Split('.');

   //                 if (s[0] == "@event")
   //                 {
   //                     o.Append(obj.@event[s[1]]);
   //                 }

   //                 if (s[0] == "@params")
   //                 {
   //                     o.Append(obj.@params[s[1]]);
   //                 }
   //                 inT = false;
   //             }
   //             else
   //             {
   //                 if (!inT && template[i] != '{' && template[i] != '}')
   //                 o.Append(template[i]);
   //             }

   //             prev = template[i];
   //         }

   //         return o.ToString();
        }
    }
}