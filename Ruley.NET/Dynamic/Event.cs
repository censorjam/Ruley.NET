using Newtonsoft.Json;
using System;

namespace Ruley
{
    public class Event : DynamicDictionary
	{
        public Event()
        {
         //   this["created"] = DateTime.UtcNow;
        }

        public Event Clone()
		{
			return (Event)JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(this));
		}
	}
}