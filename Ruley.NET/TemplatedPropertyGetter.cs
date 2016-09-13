using System;

namespace Ruley.NET
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
        private Func<dynamic, dynamic, object> _script;
        private Context _ctx;

        private string _scriptText;
        private string _scriptTemplate;

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
                _scriptText = str.Substring(2, str.Length - 2);
                _scriptText = _scriptText.Replace("'", "\"");
                _templater.Compile(_scriptText);
                return;
            }

            _type = PropertyType.Template;
            _templater.Compile(str);
            return;
        }

        private Templater _templater = new Templater();
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
                var tparams = new TemplateParameters() {@event = msg, @params = _ctx == null ? null : _ctx.Parameters};
                var script = _templater.Apply(tparams);
                object result = ScriptEngine.Create(script)(msg, _ctx == null ? null : _ctx.Parameters);
                return result;
            }

            if (_type == PropertyType.Template)
            {
                var result = _templater.Apply(new TemplateParameters() { @event = msg, @params = _ctx == null ? null : _ctx.Parameters });
                return result;
            }

            throw new Exception("error");
        }
    }

}