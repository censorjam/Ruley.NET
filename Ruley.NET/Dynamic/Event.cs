using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ruley
{
    public class Event : DynamicDictionary
	{
	    public Event()
	    {
		    
	    }

	    public Event(DynamicDictionary parameters) : base(parameters)
	    {
		    
	    }

        public Event Clone()
		{
			return (Event)JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(this));
		}
	}
}