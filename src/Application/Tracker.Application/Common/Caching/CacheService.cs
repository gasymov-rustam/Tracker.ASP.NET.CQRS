using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Tracker.Core.Primitive;

namespace Tracker.Application.Common.Caching;

public class CacheService : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> _cacheKeys = new();
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache) => _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? value = await _distributedCache.GetStringAsync(key, cancellationToken);

        return value is null ? null : JsonConvert.DeserializeObject<T>(value);
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? value = await GetAsync<T>(key, cancellationToken);

        if (value is not null)
            return value;

        value = await factory();

        await SetAsync(key, value, cancellationToken);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), cancellationToken);

        _cacheKeys.TryAdd(key, true);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        _cacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPreFixAsync(string preFix, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = _cacheKeys.Keys.Where(x => x.StartsWith(preFix)).Select(x => RemoveAsync(x, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task<List<T>> GetAllAsync<T>(Func<Task<List<T>>> factory, CancellationToken cancellationToken = default) where T : class, IBaseEntity
    {
        List<T> values = new();

        foreach (string key in _cacheKeys.Keys)
        {
            var value = await GetAsync<T>(key, cancellationToken);

            if (value is not null)
                values.Add(value);
        }

        if (values.Count != 0)
            return values;

        values = await factory();

        foreach (T value in values)
            await SetAsync(value.Id.ToString(), value, cancellationToken);

        return values;
    }
}