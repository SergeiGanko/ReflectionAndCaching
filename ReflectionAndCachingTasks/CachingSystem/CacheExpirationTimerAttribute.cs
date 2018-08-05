using System;

namespace CachingSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    class CacheExpirationTimerAttribute : Attribute
    {
        private int _seconds;

        public CacheExpirationTimerAttribute(int seconds)
        {
            _seconds = seconds;
        }
    }
}
