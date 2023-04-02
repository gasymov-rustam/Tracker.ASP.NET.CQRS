using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Infrastructure.Dal;

namespace Tracker.Infrastructure;

public static class Extension
{
  public static IServiceCollection AddDataBaseContext(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<TrackerDbContext>(options =>
        options.UseNpgsql(connectionString));

    return services;
  }
}