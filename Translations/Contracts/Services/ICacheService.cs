namespace Translations.Contracts.Services;

public interface ICacheService
{
    T GetOrAdd<T>(string key, Func<T> getFunc);
    T GetOrAdd<T>(string key, string region, Func<T> getFunc);
    void Remove(string key);
    void Clear();
    void ClearRegion(string region);
    Task<T> GetOrAddAsync<T>(string key, string region, Func<Task<T>> getFunc);
}