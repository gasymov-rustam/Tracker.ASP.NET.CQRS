using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Tracker.Application.Constants;
using Tracker.Core.Primitive;

namespace Tracker.Application.Common.Caching;

public class CacheService : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> _cacheKeys = new();
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache distributedCache, IConfiguration configuration, ILogger<CacheService> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;

        if (_cacheKeys.IsEmpty)
        {
            var config = configuration.GetConnectionString(TrackerApplicationConsts.REDIS_CONNECTION_STRING) ?? "";

            try
            {
                using ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

                foreach (var key in redis.GetServer(config).Keys())
                {
                    _cacheKeys.TryAdd(key, true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CacheService: CacheService: {Message}", e.Message);
            }
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            string? value = await _distributedCache.GetStringAsync(key, cancellationToken);

            return value is null ? null : JsonConvert.DeserializeObject<T>(value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CacheService: GetAsync: {Message}", e.Message);
            return null;
        }
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? value = await GetAsync<T>(key, cancellationToken);

        if (value is not null)
            return value;

        try
        {
            value = await factory();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CacheService: GetAsync: {Message}", e.Message);
            return null;
        }

        if (value is null)
            return null;

        var id = TrackerApplicationConsts.EMPLOYEE_REDIS_PREFIX + key;

        try
        {
            await SetAsync(id, value, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CacheService: GetAsync: {Message}", e.Message);
        }

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CacheService: SetAsync: {Message}", e.Message);
        }

        _cacheKeys.TryAdd(key, true);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CacheService: RemoveAsync: {Message}", e.Message);
        }

        _cacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPreFixAsync(string preFix, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = _cacheKeys.Keys.Where(x => x.StartsWith(preFix)).Select(x => RemoveAsync(x, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task<List<T>> GetAllByPrefixAsync<T>(string preFix, Func<Task<List<T>>> factory, CancellationToken cancellationToken) where T : class, IBaseEntity
    {
        List<T> values = new();

        if (!_cacheKeys.IsEmpty)
        {
            foreach (string key in _cacheKeys.Keys.Where(x => x.StartsWith(preFix)))
            {
                try
                {
                    var value = await GetAsync<T>(key, cancellationToken);

                    if (value is not null)
                        values.Add(value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "CacheService: GetAllByPrefixAsync: {Message}", e.Message);
                }
            }

            if (values.Count != 0)
                return values;
        }

        values = await factory();

        foreach (T value in values)
        {
            var id = TrackerApplicationConsts.EMPLOYEE_REDIS_PREFIX + value.Id;

            try
            {
                await SetAsync(id, value, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CacheService: GetAllByPrefixAsync: {Message}", e.Message);
            }
        }

        return values;
    }
}