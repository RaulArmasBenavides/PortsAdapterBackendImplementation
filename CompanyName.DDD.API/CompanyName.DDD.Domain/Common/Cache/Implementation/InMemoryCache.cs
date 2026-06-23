using Microsoft.Extensions.Caching.Memory;
namespace CompanyName.DDD.Domain.Common.Cache.Implementation
{
    public class InMemoryCache(IMemoryCache memoryCache) : IGenericCache
    {
        public T? Get<T>(string key)
        {
            if (memoryCache.TryGetValue(key, out T row))
            {
                return row;
            }
            return default;
        }
        public void Set<T>(T row, string key)
        {
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            memoryCache.Set(key, row, cacheOptions);
        }
    }
}