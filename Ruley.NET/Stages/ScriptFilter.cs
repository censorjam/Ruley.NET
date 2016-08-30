using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Ruley.NET
{
    public class ScriptStage : InlineStage
    {
        [Primary]
        public Property<string> Value { get; set; }
        private Func<dynamic, dynamic, object> _script = null;

        public override void OnFirst(Event e)
        {
            var s = Value.Get(e);
            _script = ScriptEngine.Create(s);
        }

        public override Event Apply(Event msg)
        {
            _script(msg, Context.Parameters);
            return msg;
        }
    }
}
