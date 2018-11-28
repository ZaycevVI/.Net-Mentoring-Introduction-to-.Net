using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FibonacciCaching.Cache
{
    public class FibonacciRedisCache : IFibonacciCache
    {
        private const string Host = "localhost";
        private readonly ConnectionMultiplexer _redisConnection;
        private const string Key = "Redis_Fibonacci";

        public FibonacciRedisCache()
        {
            _redisConnection = ConnectionMultiplexer.Connect(Host);
        }

        public List<ulong> Get(string step)
        {
            var db = _redisConnection.GetDatabase();
            string str = db.StringGet(Key);

            return str == null ? null : JsonConvert.DeserializeObject<List<ulong>>(str);
        }

        public void Set(string step, List<ulong> result)
        {
            var db = _redisConnection.GetDatabase();
            var resultString = JsonConvert.SerializeObject(result);
            db.StringSet(Key, resultString);
        }
    }
}
