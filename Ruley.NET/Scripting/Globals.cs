using System;

namespace Ruley.NET
{
    public class Globals
    {
        public Globals(Event ev, DynamicDictionary pars)
        {
            @event = ev;
            @params = pars;
        }

        public dynamic @event { get; private set; }
        public dynamic @params { get; private set; }

        public long month(DateTime dt)
        {
            return dt.Month;
        }
    }
}