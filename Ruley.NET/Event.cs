using System;
using System.Collections.Generic;
using Ruley.Dynamic;

namespace Ruley.Core
{
    public class Event
    {
        public DynamicDictionary Data { get; set; }

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