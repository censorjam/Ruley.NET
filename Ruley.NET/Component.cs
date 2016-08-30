using System;
using Newtonsoft.Json;

namespace Ruley.NET
{
    public abstract class Component : IDisposable
    {
        public Component()
        {
            Logger = new Logger(GetType().Name.Replace("Stage", ""), false);
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

		public Context Context { get; set; }

        private bool _debug;
        public bool Debug
        {
            get
            {
                return Logger.IsDebugEnabled;
            }
            set
            {
                _debug = value;
                Logger = new Logger(GetType().Name.Replace("Stage", ""), _debug);
            }
        }

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