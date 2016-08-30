using Newtonsoft.Json;

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

        public Event Set(string key, object value)
        {
            this[key] = value;
            return this;
        }
	}
}