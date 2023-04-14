using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Shared.Constants;

namespace Tracker.Shared.ResponseCache;

internal static class CacheExtension
{
    internal static IServiceCollection AddResponseCache(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheOptions = configuration.GetSection(TrackerOptionsConsts.RESPONSE_CACHE_OPTIONS).GetChildren();

        if (cacheOptions is null)
            return services;

        services.AddControllers(options =>
        {
            foreach (var cacheProfile in cacheOptions)
            {
                options.CacheProfiles.Add(cacheProfile.Key, cacheProfile.Get<CacheProfile>());
            }
        });

        return services;
    }
}
