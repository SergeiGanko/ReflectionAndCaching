using System;
using System.Threading;

namespace CachingSystem
{
    internal sealed class Program
    {
        private static void Main()
        {
            var model = new Engine
            {
                Manufacturer = "Mercedes",
                Model = "W111",
                Power = 550
            };

            var key = "key";

            var json = ReflectionSerializer.Serialize(model);
            var time = ReflectionSerializer.GetExpirationTime(model);

            RedisCache.SaveInCache(key, json, time);

            Console.WriteLine($"Data saved in cache with key: {key}\n" +
                              $"Cache expiration time: {time}\n" +
                              $"Model: {model.Manufacturer}, {model.Model}, {model.Power}\n" +
                              $"Model after serialization: {json}\n");

            //Thread.Sleep(new TimeSpan(0, 0, 6));

            var cacheData = RedisCache.ReadFromCache(key);

            if (cacheData != null)
            {
                var result = ReflectionSerializer.Deserialize<Engine>(cacheData);

                Console.WriteLine($"Data from cache: {cacheData}\n" +
                                  $"Model after deserialization: {result.Manufacturer}, {result.Model}, {result.Power}\n");
            }
            else
            {
                Console.WriteLine($"Time expired! Cache is empty.");
            }

            Console.ReadLine();
        }
    }
}
