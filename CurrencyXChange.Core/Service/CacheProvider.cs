using System;
using System.Collections.Generic;
using System.Text;
using CurrencyXChange.Core.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyXChange.Core.Service
{
    public class CacheProvider : ICacheProvider
    {
        private const int CacheSeconds = 10; // 10 Seconds

        private readonly IMemoryCache _cache;

        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            var cachedResponse = _cache.Get(key);
            return cachedResponse as T;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            SetCache(key, value, DateTimeOffset.Now.AddSeconds(CacheSeconds));
        }

        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class
        {
            _cache.Set(key, value, duration);
        }

        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
