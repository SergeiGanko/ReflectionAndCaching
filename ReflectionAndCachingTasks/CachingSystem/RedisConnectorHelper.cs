using System;
using StackExchange.Redis;

namespace CachingSystem
{
    /// <summary>
    /// RedisCache class
    /// </summary>
    public class RedisCache
    {
        private static readonly Lazy<ConnectionMultiplexer> lazyConnection;

        /// <summary>
        /// Initializes the <see cref="RedisCache"/> class.
        /// </summary>
        static RedisCache()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public static ConnectionMultiplexer Connection => lazyConnection.Value;

        /// <summary>
        /// Reads data from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns string</returns>
        public static string ReadFromCache(string key)
        {
            var cache = Connection.GetDatabase();

            return cache.StringGet(key);
        }

        /// <summary>
        /// Saves data in cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expirationTime">The expiration time.</param>
        public static void SaveInCache(string key, string value, TimeSpan expirationTime)
        {
            var cache = Connection.GetDatabase();

            cache.StringSet(key, value, expirationTime);
        }
    }
}
