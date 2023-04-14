using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Common.Pipeline;
using FluentValidation;
using System.Reflection;
using Tracker.Application.Common.Caching;

namespace Tracker.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(x => x.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
