using System;
using Newtonsoft.Json;
using Ruley.Dynamic;

namespace Ruley.Core.Outputs
{
    public abstract class Component : IDisposable
    {
        public Rule Context { get; internal set; }

        public Component()
        {
            Logger = new Logger();
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool Debug { get; set; }

        public Logger Logger { get; set; }

        public Property<bool> If { get; set; } 

        public virtual void Dispose()
        {
        }

        public virtual void ValidateComposition()
        {
            if (Context == null)
                throw new ArgumentNullException("Context is null");
        }

        //protected T Get<T>(object value, Event msg)
        //{
        //    var getter = new TemplatedPropertyGetter(value);
        //    return getter.Get<T>(value, msg.Data);
        //}
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}