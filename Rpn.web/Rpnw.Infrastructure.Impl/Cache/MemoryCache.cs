using Microsoft.Extensions.Caching.Memory;
using Rpnw.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Infrastructure.Impl.Cache
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        public bool TryGetValue<T>(string key, out T value) 
        {
            value = Get<T>(key);
            if (value != null)
                return true;

            return false;
        }

        public bool Remove(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }

        public void Set<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (absoluteExpireTime.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpireTime.Value);
            }

            if (unusedExpireTime.HasValue)
            {
                options.SetSlidingExpiration(unusedExpireTime.Value);
            }

            _memoryCache.Set(key, value, options);
        }
    }
}
