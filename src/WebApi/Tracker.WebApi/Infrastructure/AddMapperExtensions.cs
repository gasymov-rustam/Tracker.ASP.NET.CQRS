using Mapster;
using MapsterMapper;
using Tracker.WebApi.Infrastructure.Mappers;

namespace Tracker.WebApi.Infrastructure;

public static class AddMapperExtensions
{
    private static TypeAdapterConfig GetConfigureMappingConfig()
    {
        var config = new TypeAdapterConfig();

        // TODO: register scope
        new ProjectMapper().Register(config);

        return config;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddSingleton(GetConfigureMappingConfig());
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
