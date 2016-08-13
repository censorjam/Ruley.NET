using System;

namespace Ruley
{
    public class Event
    {
        public DynamicDictionary Parameters { get; set; }
        public DynamicDictionary Data { get; set; }

        public dynamic @params { get
            {
                return Parameters;
            }
            set
            {
            }
        }

        public dynamic @event
        {
            get
            {
                return Data;
            }
            set
            {
            }
        }

        public Event()
        {
            Data = new DynamicDictionary();
            Data["$created"] = DateTime.UtcNow;
        }

        public Event(DynamicDictionary data)
        {
            Data = data;
            Data["$created"] = DateTime.UtcNow;

        }

        internal static Event Create(DynamicDictionary data)
        {
            data["$created"] = DateTime.UtcNow;
            return new Event(data);
        }
    }
}