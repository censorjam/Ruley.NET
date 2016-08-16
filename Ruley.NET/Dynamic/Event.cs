using Newtonsoft.Json;
using System;

namespace Ruley
{
    public class Event : DynamicDictionary
	{
        public Event Clone()
		{
			return (Event)JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(this));
		}
	}
}