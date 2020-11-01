using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Web
{
    public class CacheBase : ICacheBase
    {
        private static IMemoryCache _cache;
        public CacheBase(IMemoryCache cache)
        {
            _cache = cache;
        }
        public CacheBase() { }
        public T Get<T>(string key)
        {
            _cache.TryGetValue<T>(key, out var value);
            return value;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(string key, T value)
        {
            _cache.Set<T>(key, value);
        }
    }
}
