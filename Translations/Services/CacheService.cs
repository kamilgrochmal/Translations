using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Translations.Contracts.Services;

namespace Translations.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private const string DefaultCacheName = "defaultCache";
    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }
    public T GetOrAdd<T>(string key, Func<T> getFunc)
    {
        if(key == DefaultCacheName) throw  new ArgumentException(DefaultCacheName + " is reserved. Please use another key", nameof(key));
        return _cache.GetOrCreate(key, entry =>
        {
            var source = _cache.GetOrCreate(DefaultCacheName, e => new CancellationTokenSource());
            entry.AddExpirationToken(new CancellationChangeToken(source.Token));
            return getFunc();
        });
    }
    public T GetOrAdd<T>(string key, string region, Func<T> getFunc)
    {
        if(key == DefaultCacheName) throw  new ArgumentException(DefaultCacheName + " is reserved. Please use another key", nameof(key));
        if(region == DefaultCacheName) throw  new ArgumentException(DefaultCacheName + " is reserved. Please use another key", nameof(region));
        return _cache.GetOrCreate(key, entry =>
        {
            var source = _cache.GetOrCreate(region, e => new CancellationTokenSource());
            entry.AddExpirationToken(new CancellationChangeToken(source.Token));
            return getFunc();
        });
    }

    public async Task<T> GetOrAddAsync<T>(string key, string region, Func<Task<T>> getFunc)
    {
        if(key == DefaultCacheName) throw  new ArgumentException(DefaultCacheName + " is reserved. Please use another key", nameof(key));
        if(region == DefaultCacheName) throw  new ArgumentException(DefaultCacheName + " is reserved. Please use another key", nameof(region));
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            var source = _cache.GetOrCreate(region, e => new CancellationTokenSource());
            entry.AddExpirationToken(new CancellationChangeToken(source.Token));
            return await getFunc();
        });
    }
    public void Remove(string key)
    {
        _cache.Remove(key);
    }
    public void Clear()
    {
        var source = _cache.Get<CancellationTokenSource>(DefaultCacheName);
        source.Cancel();
    }
    public void ClearRegion(string region)
    {
        var source = _cache.Get<CancellationTokenSource>(region);
        source.Cancel();
    }
}