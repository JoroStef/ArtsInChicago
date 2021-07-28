using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{
    public class CachHelper
    {
        private const int defaultExpMinutes = 1;

        public static void CachInMemory<T>(T obj, string key, IMemoryCache memoryCache, int expMinutes = defaultExpMinutes)
        {
            memoryCache.Set(key, obj, TimeSpan.FromMinutes(expMinutes));

            //using (var cacheEntry = memoryCache.CreateEntry(key))
            //{
            //    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes((double)defaultExpMinutes);
            //    cacheEntry.SetValue(obj);
            //}
        }

        public static (int pageNumber, T obj) GetCachedInMemory<T>(IMemoryCache memoryCache, string keyPage, string keyPageNumber)
        {
            if (memoryCache.TryGetValue(keyPage, out T newModel))
            {
                return (default, newModel);
            }

            if (!memoryCache.TryGetValue(keyPageNumber, out int page))
            {
                page = 1;
            }
            return (page, default);

        }
    }
}
