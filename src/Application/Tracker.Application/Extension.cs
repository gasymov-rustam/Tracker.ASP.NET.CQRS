using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Common.Mappers;

namespace Tracker.Application;

public static class Extension
{
  private static TypeAdapterConfig GetConfigureMappingConfig()
  {
    var config = new TypeAdapterConfig();

    // TODO: register scope
    new ProjectMapper().Register(config);
    // new RolesMapper().Register(config);
    // new HoldersMapper().Register(config);
    // new ExternalMapper().Register(config);
    // new ResultsMapper().Register(config);
    // new UsersManagementMapper().Register(config);

    return config;
  }

  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddSingleton(GetConfigureMappingConfig());
    services.AddScoped<IMapper, ServiceMapper>();
    services.AddMediator(x => x.ServiceLifetime = ServiceLifetime.Scoped);

    return services;
  }
}
