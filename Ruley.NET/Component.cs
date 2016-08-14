using System;
using Newtonsoft.Json;

namespace Ruley
{
    public abstract class Component : IDisposable
    {
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

		public Context Ctx { get; set; }

        public bool Debug { get; set; }

        public Logger Logger { get; set; }

        public Property<bool> If { get; set; } 

        public virtual void Dispose()
        {
        }

        public virtual void ValidateComposition()
        {
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