using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Concurrent;

namespace Ruley.NET
{
    public class ScriptException : Exception
    {
        public ScriptException(string message, Exception inner): base(message, inner)
        {
        }
    }

    public class ScriptEngine
    {
        public static ConcurrentDictionary<string, Script<object>> Cache = new ConcurrentDictionary<string, Script<object>>();

        public static Func<dynamic, dynamic, object> Create(string script)
        {
            var scriptOptions = ScriptOptions.Default
                .WithImports("System")
                .WithReferences("Microsoft.CSharp");

            var _script = Cache.GetOrAdd(script, (k) =>
            {
                return CSharpScript.Create<object>(script, globalsType: typeof(Globals), options: scriptOptions);
            });

            return new Func<dynamic, dynamic, object>((e, p) =>
            {
                //try
                //{
                    return _script.RunAsync(new Globals(e, p)).Result.ReturnValue;
                //}
                //catch (CompilationErrorException ex)
                //{
                //    var msg = ex.Message;
                //    var parts = msg.Split('\'');

                //    if (msg.Contains("CS0103") && parts.Length == 2)
                //    {
                //        msg += $". Did you mean @event.{parts[1]} or @params.{parts[1]}?";
                //        var scriptException = new ScriptException(msg, ex);
                //        throw scriptException;
                //    }

                //    throw ex;
                //}
            });
        }
    }
}
