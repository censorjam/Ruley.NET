using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace Ruley
{
	public class DynamicDictionary : DynamicObject, IDictionary<string, object>
	{
		protected readonly IDictionary<string, object> _data = new Dictionary<string, object>();

		public DynamicDictionary()
		{
		}

        public DynamicDictionary(string json)
        {
            _data = (DynamicDictionary)JsonHelper.Deserialize(json);
        }

        public DynamicDictionary(IDictionary<string, object> source)
		{
			_data = source;
		}

		public void Add(KeyValuePair<string, object> item)
		{
			_data.Add(item);
		}

		public void Clear()
		{
			_data.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return _data.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			_data.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return _data.Remove(item);
		}

		public int Count
		{
			get
			{
				return _data.Count;
			}
		}

		public bool IsReadOnly
		{
			get { return _data.IsReadOnly; }
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _data.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			_data[binder.Name] = value;
			return true;
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_data).GetEnumerator();
		}

		public bool ContainsKey(string key)
		{
			return _data.ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			_data.Add(key, value);
		}

		public bool Remove(string key)
		{
			return _data.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return _data.TryGetValue(key, out value);
		}

		public virtual object this[string key]
		{
			get
			{
				return _data[key];
			}
			set
			{
				_data[key] = value;
			}
		}

		public ICollection<string> Keys
		{
			get { return _data.Keys; }
		}

		public ICollection<object> Values
		{
			get { return _data.Values; }
		}

		public object GetValue(string path)
		{
			try
			{
				IDictionary<string, object> o = this;
				var split = path.Split('.');

				for (var s = 0; s < split.Length; s++)
				{
					if (s == split.Length - 1)
					{
						return o[split[s]];
					}
					else
					{
						o = (IDictionary<string, object>)o[split[s]];
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("GetValue failed with path '{0}'", path));
			}

			throw new Exception("broken");
		}

		public object SetValue(string path, object value)
		{
			return _data[path] = value;
		}

		public void Merge(Event other)
		{
			foreach (var o in other)
			{
				this[o.Key] = o.Value;
			}
		}

		public bool HasProperty(string property)
		{
			return _data.ContainsKey(property);
		}

        protected bool Equals(DynamicDictionary other)
        {
            if (Keys.Count != other.Keys.Count())
                return false;

            foreach (var key in Keys)
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

        //public static Event Create(object o)
        //{
        //	var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
        //	return (Event)JsonHelper.Deserialize(JsonConvert.SerializeObject(o, settings));
        //}

        //public static Event Create(string json)
        //{
        //	return (Event)JsonHelper.Deserialize(json);
        //}
    }
}
