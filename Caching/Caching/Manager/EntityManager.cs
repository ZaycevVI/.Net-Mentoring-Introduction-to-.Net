using System;
using System.Collections.Generic;
using System.Linq;
using Caching.Caching;
using NorthwindLibrary;

namespace Caching.Manager
{
    public class EntityManager<T> : IDisposable where T : class, new()
    {
        protected readonly SqlMonitorMemoryCache<T> Cache;

        public EntityManager(string tableName)
        {
            Cache = new SqlMonitorMemoryCache<T>(tableName);
        }

        public IEnumerable<T> GetCategories()
        {
            Console.WriteLine($"Get {typeof(T)}");

            var entities = Cache.Get();

            if (entities != null)
            {
                Console.WriteLine("Extracted from cache.");
                return entities;
            }
            Console.WriteLine("From DB");

            using (var dbContext = new Northwind())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                dbContext.Configuration.ProxyCreationEnabled = false;
                entities = dbContext.Set<T>().ToList();
                Console.WriteLine("Save data to cache.");
                Cache.Set(entities);
            }

            Console.WriteLine("Extracted from DB.");
            return entities;
        }

        public void Dispose()
        {
            Cache.Dispose();
        }
    }
}
