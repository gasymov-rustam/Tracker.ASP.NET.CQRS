using Tracker.Core.Primitive;

namespace Tracker.Application.Common.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> GetAllAsync<T>(Func<Task<List<T>>> factory, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPreFixAsync(string preFix, CancellationToken cancellationToken = default);
}