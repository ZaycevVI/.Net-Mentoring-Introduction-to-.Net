using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace FibonacciCaching.Cache
{
    public class FibonacciMemoryCache : IFibonacciCache
    {
        private readonly ObjectCache _cache;

        public FibonacciMemoryCache()
        {
            _cache = MemoryCache.Default;
        }

        public List<ulong> Get(string step)
        {
            return (List<ulong>)_cache.Get(step);
        }

        public void Set(string step, List<ulong> result)
        {
            _cache.Set(step, result, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}
