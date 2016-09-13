using System;

namespace Ruley.NET
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.UtcNow;
        }
    }
}