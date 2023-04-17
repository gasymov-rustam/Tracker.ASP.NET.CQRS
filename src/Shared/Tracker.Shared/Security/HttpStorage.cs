using Microsoft.AspNetCore.Http;

namespace Tracker.Shared.Security;

public interface IStorage
{
    void Set<TEntity>(TEntity entity, string key);
    TEntity? Get<TEntity>(string key);
}

public class HttpStorage : IStorage
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpStorage(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public void Set<TEntity>(TEntity entity, string key)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(key, entity);

    public TEntity? Get<TEntity>(string key)
    {
        if (_httpContextAccessor.HttpContext is null) return default;

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(key, out var entity))
        {
            if (entity is TEntity tEntity) return tEntity;
        }

        return default;
    }
}