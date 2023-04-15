using Tracker.Core.Primitive;

namespace Tracker.Application.Common.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> GetAllByPrefixAsync<T>(string preFix, Func<Task<List<T>>> factory, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPreFixAsync(string preFix, CancellationToken cancellationToken = default);
}