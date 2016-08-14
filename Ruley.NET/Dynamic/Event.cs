using Newtonsoft.Json;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Ruley
{
	public class Event : DynamicDictionary
	{
		private ImmutableDictionary<string, object> _parameters;

		public Event()
		{
			_parameters = ImmutableDictionary<string, object>.Empty;
			_parameters = _parameters.SetItem("$created", DateTime.UtcNow);
		}

		public Event(ImmutableDictionary<string, object> parameters)
		{
			_parameters = parameters.SetItem("$created", DateTime.UtcNow);
		}

		protected bool Equals(Event other)
		{
			//todo dont need this any more
			var keys = Keys.Where(k => !k.StartsWith("$")).ToList();
			var otherKeys = other.Keys.Where(k => !k.StartsWith("$"));

			if (keys.Count != otherKeys.Count())
				return false;

			foreach (var key in keys)
			{
				object o;
				var found = other.TryGetValue(key, out o);

				if (!found)
					return false;

				if (!this[key].Equals(o))
				{
					return false;
				}
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Event)obj);
		}

		public override int GetHashCode()
		{
			return (_data != null ? _data.GetHashCode() : 0);
		}

		public override object this[string key]
		{
			get
			{
				if (key.StartsWith("$"))
				{
					return _parameters[key];
				}

				return _data[key];
			}
			set
			{
				if (key.StartsWith("$"))
					throw new ArgumentException("{key} is immutable");

				_data[key] = value;
			}
		}

		public Event Clone()
		{
			return (Event)JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(this));
		}
	}
}