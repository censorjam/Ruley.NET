using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Text;

namespace Ruley.Templating
{
    public class Templater
    {
        private List<ValueGetter> f = new List<ValueGetter>();

        public Templater()
        {
        }

        public string Apply(object data)
        {
            object[] p = new object[f.Count];
            for (var i = 0; i < f.Count; i++)
            {
                p[i] = f[i].Get(data);
            }
            return string.Format(_formatString, p);
        }

        private string _formatString;

        public void Compile(string template)
        {
            bool inToken = false;
            StringBuilder stringformat = new StringBuilder();
            StringBuilder token = new StringBuilder();

            int tokenCount = 0;

            for (var i = 0; i < template.Length; i++)
            {
                if (template[i] == '{')
                {
                    inToken = true;
                    continue;
                }

                if (template[i] == '}')
                {
                    var parts = token.ToString().Split(':');
                    var t = new ValueGetter(parts[0]);
                    f.Add(t);
                    token.Clear();

                    if (parts.Length > 1)
                        stringformat.Append("{" + tokenCount + ":" + parts[1] + "}");
                    else
                        stringformat.Append("{" + tokenCount + "}");

                    tokenCount++;

                    inToken = false;
                    continue;
                }
                if (!inToken)
                    stringformat.Append(template[i]);
                else
                {
                    token.Append(template[i]);
                }
            }

            _formatString = stringformat.ToString();
        }
    }


    public class ValueGetter
    {
        private string[] _path;

        public ValueGetter(string path)
        {
            _path = path.Split('.');
        }

        private static ConcurrentDictionary<Type, CallSite<Func<CallSite, object, object>>> _cache = new ConcurrentDictionary<Type, CallSite<Func<CallSite, object, object>>>();

        static object GetDynamicMember(object obj, string memberName)
        {
            var callsite = _cache.GetOrAdd(obj.GetType(), t =>
            {
                var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, memberName, obj.GetType(),
                    new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
                return CallSite<Func<CallSite, object, object>>.Create(binder);
            });
            return callsite.Target(callsite, obj);
        }

        public object Resolve(object o, string memberName)
        {
            if (o is IDynamicMetaObjectProvider)
            {
                return (GetDynamicMember(o, memberName));
            }

            var p = o.GetType().GetProperty(memberName);

            if (p != null)
                return p.GetValue(o);

            var f = o.GetType().GetField(memberName);

            if (f != null)
                return f.GetValue(o);

            return null;
        }

        public object Get(object o)
        {
            foreach (var s in _path)
            {
                o = Resolve(o, s);

                if (o == null)
                    return null;
            }
            return o;
        }
    }
}