using System;
using System.Collections.Generic;
using FibonacciCaching.Cache;

namespace FibonacciCaching.Service
{
    public class FibonacciService : IFibonacciService
    {
        private const string CacheKey = "mentoring_fibonacci";
        private readonly IFibonacciCache _cache;
        private readonly bool _isCacheEnabled;

        public FibonacciService(IFibonacciCache cache, bool isCacheEnabled)
        {
            _cache = cache;
            _isCacheEnabled = isCacheEnabled;
        }

        public List<ulong> Calculate(uint step)
        {
            var intStep = Convert.ToInt32(step);

            var resultSet = new List<ulong> {0, 1};

            if (step < 2)
                return new List<ulong> {0};

            if (step == 2)
                return resultSet;

            var cacheStep = 2;

            if (_isCacheEnabled)
            {
                resultSet = _cache.Get(CacheKey) ?? resultSet;
                cacheStep = resultSet.Count;

                if (cacheStep >= step)
                    return resultSet.GetRange(0, intStep);
            }

            for (var i = cacheStep; i < step; i++)
                resultSet.Add(resultSet[i -1] + resultSet[i - 2]);

            if (_isCacheEnabled)
                _cache.Set(CacheKey, resultSet);

            return resultSet;
        }
    }
}