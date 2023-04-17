using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Common.Interfaces;
using Tracker.Infrastructure.Dal;
using Tracker.Infrastructure.Dal.Initializers;

namespace Tracker.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddDataBaseContext(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ITrackerDBContext, TrackerDbContext>();

        services.AddDbContext<TrackerDbContext>(options =>
                                options.UseNpgsql(connectionString,
                                b => b.MigrationsAssembly("Tracker.WebApi")));

        services.AddHostedService<DatabaseInitializer<TrackerDbContext>>();
        services.AddHostedService<DataInitializer>();
        services.AddInitializer<RoleInitiallizer>();
        services.AddInitializer<EmployeeInitiallizer>();
        services.AddInitializer<UserInitiallizer>();
        services.AddTransient<IDataInitializer, RoleInitiallizer>();

        return services;
    }

    public static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
         => services.AddTransient<IDataInitializer, T>();
}