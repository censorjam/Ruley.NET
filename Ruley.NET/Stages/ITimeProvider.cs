using System;

namespace Ruley.NET
{
    public interface ITimeProvider
    {
        DateTime GetNow();
    }
}