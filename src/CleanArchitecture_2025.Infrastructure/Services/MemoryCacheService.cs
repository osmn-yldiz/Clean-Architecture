using CleanArchitecture_2025.Application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture_2025.Infrastructure.Services;

internal sealed class MemoryCacheService(
    IMemoryCache cache) : ICacheService
{
    private static readonly HashSet<string> CacheKeys = new();

    public T? Get<T>(string key)
    {
        cache.TryGetValue<T>(key, out var value);

        return value;
    }

    public bool Remove(string key)
    {
        cache.Remove(key);

        return true;
    }

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(1),
        };

        cache.Set<T>(key, value, cacheEntryOptions);

        CacheKeys.Add(key);
    }

    public void RemoveAll()
    {
        foreach (var key in CacheKeys)
        {
            cache.Remove(key);
        }

        CacheKeys.Clear();
    }
}
