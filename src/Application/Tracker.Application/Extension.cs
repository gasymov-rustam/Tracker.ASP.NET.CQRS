using Microsoft.Extensions.DependencyInjection;

namespace Tracker.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(x => x.ServiceLifetime = ServiceLifetime.Scoped);

        return services;
    }
}
