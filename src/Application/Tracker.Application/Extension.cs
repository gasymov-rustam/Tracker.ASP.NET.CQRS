using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Common.Pipeline;

namespace Tracker.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(x => x.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ErrorLoggingBehaviour<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));

        return services;
    }
}
