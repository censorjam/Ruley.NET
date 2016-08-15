using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Concurrent;

namespace Ruley.Core.Outputs
{
    public static class RoselynCache
    {
        public static ConcurrentDictionary<string, Script<object>> Cache = new ConcurrentDictionary<string, Script<object>>();
    }

    public class TemplatedPropertyGetter
    {
        public enum PropertyType
        {
            Value,
            Field,
            Template,
            Eval
        }

        private PropertyType _type;
        private string _fieldName;
        private Script<object> _script;
        private Context _ctx;

        public TemplatedPropertyGetter(Context ctx, object value)
        {
            _ctx = ctx;
            SetPropertyType(value);
        }

        private void SetPropertyType(object value)
        {
            var str = value as string;
            if (str == null)
            {
                _type = PropertyType.Value;
                return;
            }

            if (str.StartsWith("=>"))
            {
                _type = PropertyType.Eval;
                _fieldName = str.Substring(2, str.Length - 2);
                _fieldName = _fieldName.Replace("'", "\"");

                var scriptOptions = ScriptOptions.Default
                    .WithImports("System")
                    .WithReferences("Microsoft.CSharp");

                _script = RoselynCache.Cache.GetOrAdd(_fieldName, (k) =>
                {
                    return CSharpScript.Create<object>(_fieldName, globalsType: typeof(Globals), options: scriptOptions);
                });

                return;
            }

            _type = PropertyType.Template;
            _fieldName = str;//.Substring(5, str.Length - 6);
            return;

            _type = PropertyType.Value;
        }

        public object GetValue(object value, Event msg)
        {
            if (_type == PropertyType.Value)
            {
                return value;
            }
        
            if (_type == PropertyType.Field)
            {
                return msg.GetValue(_fieldName);
            }

            if (_type == PropertyType.Eval)
            {
                var g = new Globals();
                g.@event = msg;
                g.@params = _ctx.Parameters;
                object result = _script.RunAsync(g).Result.ReturnValue;
                return result;
            }

            if (_type == PropertyType.Template)
            {
                var result = Templater.ApplyTemplate(_fieldName, msg);
                return result;
            }

            throw new Exception("error");
        }
    }

    public class Globals
    {
        public dynamic @event;
        public dynamic @params;
    }
}