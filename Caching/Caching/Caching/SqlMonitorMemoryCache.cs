using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Delegates;

namespace Caching.Caching
{
    public class SqlMonitorMemoryCache<T> : IDisposable where T : class, new()
    {
        protected readonly ObjectCache Cache = MemoryCache.Default;
        protected readonly SqlTableDependency<T> Dependency;
        private readonly string _key;

        public event ChangedEventHandler<T> TableChanged
        {
            add => Dependency.OnChanged += value;
            remove => Dependency.OnChanged -= value;
        }

        public SqlMonitorMemoryCache(string tableName)
        {
            _key = typeof(T).FullName;
            Dependency = new SqlTableDependency<T>(ConfigurationManager
                .ConnectionStrings["Northwind"]
                .ConnectionString, tableName,
                executeUserPermissionCheck: false);
            Dependency.OnChanged += (sender, args) => Cache.Remove(_key);
            Dependency.Start();
        }

        public IEnumerable<T> Get()
        {
            return (IEnumerable<T>)Cache.Get(_key);
        }

        public void Set(IEnumerable<T> entities, int seconds = 30)
        {
            Cache.Set(_key, entities, DateTimeOffset.UtcNow.AddSeconds(seconds));
        }

        public void Dispose()
        {
            Dependency.Stop();
        }
    }
}
