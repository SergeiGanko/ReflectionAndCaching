namespace CachingSystem
{
    [CacheExpirationTimer(5)]
    public class Engine
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Power { get; set; }
    }
}
