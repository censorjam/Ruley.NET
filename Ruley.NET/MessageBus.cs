using System;
using System.Collections.Generic;

namespace Ruley.Core
{
    public class MessageBus
    {
        public void Publish(string key, Event msg)
        {
            lock (_subs)
            {
                List<Action<Event>> subs;
                _subs.TryGetValue(key, out subs);

                if (subs != null)
                {
                    foreach (var s in subs)
                    {
                        s(msg);
                    }
                }
            }
        }

        private Dictionary<string, List<Action<Event>>> _subs = new Dictionary<string, List<Action<Event>>>();
        public void Subscribe(string key, Action<Event> handler)
        {
            lock (_subs)
            {
                List<Action<Event>> subs;
                _subs.TryGetValue(key, out subs);

                if (subs != null)
                {
                    subs.Add(handler);
                }
                else
                {
                    _subs.Add(key, new List<Action<Event>>() { handler });
                }
            }
        }
    }
}