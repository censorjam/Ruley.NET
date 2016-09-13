using System;
using Newtonsoft.Json;

namespace Ruley.NET
{
    public abstract class Component : IDisposable
    {
        public Component()
        {
            Logger = new Logger()
            {
                Name = GetType().Name.Replace("Stage", ""),
                IsDebugEnabled = false
            };
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
                Logger.IsDebugEnabled = value;
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}