using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetRu.Models.Social;
using Microsoft.Extensions.Caching.Memory;

namespace DotNetRu.Azure
{
    internal static class CacheHelper
    {
        internal static readonly MemoryCacheWithPolicy<IList<ISocialPost>> PostsCache = new MemoryCacheWithPolicy<IList<ISocialPost>>();

        private static int _expirationInMinutes;

        /// <summary>
        /// Set inmemory cache expiration in minutes.
        /// </summary>
        /// <param name="minutes">Minutes.</param>
        internal static void SetExpirationInMinutes(int minutes)
        {
            _expirationInMinutes = minutes;
        }

        internal class MemoryCacheWithPolicy<TItem>
        {
            private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 100 // ограничение размера кэш-контейнера по кол-ву объектов с учётом "веса" каждого объекта
            });

            static MemoryCacheWithPolicy()
            {
                _expirationInMinutes = 5;
            }

            public async Task<TItem> GetOrCreateAsync(object key, Func<Task<TItem>> createItem)
            {
                if (createItem == null)
                    return default;

                if (!cache.TryGetValue(key, out TItem cacheEntry))
                {
                    cacheEntry = await createItem();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // устанавливаем "вес" добавляемого объекта, который учитывается в ограничении по размеру
                        .SetSize(1)
                        // приоритет на удаление при достижении предельного размера (давления на память)
                        .SetPriority(CacheItemPriority.High)
                        // удаляем из кэша по истечении этого времени, независимо от скользящего таймера.
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(_expirationInMinutes));

                    cache.Set(key, cacheEntry, cacheEntryOptions);
                }
                return cacheEntry;
            }
        }
    }
}
