using System;
using Ruley.Dynamic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Ruley.Core.Outputs
{
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

        public TemplatedPropertyGetter(object value)
        {
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

            //if (str.StartsWith("eval(") && str.EndsWith(")"))
            //{
            //    _type = PropertyType.Eval;
            //    _fieldName = str.Substring(5, str.Length - 6);

            //    var scriptOptions = ScriptOptions.Default
            //        .WithImports("System")
            //        .WithReferences("Microsoft.CSharp");

            //    _script = CSharpScript.Create<object>(_fieldName, globalsType: typeof(Globals), options: scriptOptions);

            //    return;
            //}

            //if (str.StartsWith("field(") && str.EndsWith(")"))
            //{
            //    _type = PropertyType.Field;
            //    _fieldName = str.Substring(6, str.Length - 7);
            //    return;
            //}

            if (str.StartsWith("=>"))
            {
                _type = PropertyType.Eval;
                _fieldName = str.Substring(2, str.Length - 2);
                _fieldName = _fieldName.Replace("'", "\"");

                var scriptOptions = ScriptOptions.Default
                    .WithImports("System")
                    .WithReferences("Microsoft.CSharp");

                _script = CSharpScript.Create<object>(_fieldName, globalsType: typeof(Globals), options: scriptOptions);

                return;
            }

            //if (str.StartsWith("text(") && str.EndsWith(")"))
            {
                _type = PropertyType.Template;
                _fieldName = str;//.Substring(5, str.Length - 6);
                return;
            }
            //else
            

            _type = PropertyType.Value;
        }

        public object GetValue(object value, DynamicDictionary msg)
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
    }
}