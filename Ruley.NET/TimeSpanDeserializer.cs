using System;

namespace Ruley
{
    public class TimeSpanDeserializer
    {
        public static TimeSpan Deserialize(string ts)
        {
            if (ts.EndsWith("ms"))
            {
                ts = ts.Substring(0, ts.Length - 2);
                return TimeSpan.FromMilliseconds(double.Parse(ts));
            }

            if (ts.EndsWith("s"))
            {
                ts = ts.Substring(0, ts.Length - 1);
                return TimeSpan.FromSeconds(double.Parse(ts));
            }

            if (ts.EndsWith("m"))
            {
                ts = ts.Substring(0, ts.Length - 1);
                return TimeSpan.FromMinutes(double.Parse(ts));
            }

            if (ts.EndsWith("h"))
            {
                ts = ts.Substring(0, ts.Length - 1);
                return TimeSpan.FromMinutes(double.Parse(ts));
            }

            throw new Exception("Unknown");
        }
    }
}