using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CachingSystem
{
    public class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        }

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection => lazyConnection.Value;
    }
}
