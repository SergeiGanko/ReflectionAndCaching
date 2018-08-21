using System;

namespace CachingSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    class CacheExpirationTimerAttribute : Attribute
    {
        public CacheExpirationTimerAttribute(int seconds)
        {
            Seconds = seconds;
        }

        public int Seconds { get; }
    }
}
