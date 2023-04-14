using Tracker.Shared.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Shared.Exceptions;
using Tracker.Shared.ResponseCache;

namespace Tracker.Shared;
public static class Extension
{
    public static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    => BindOptions<T>(configuration.GetSection(sectionName));

    public static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }

    public static WebApplicationBuilder AddShared(this WebApplicationBuilder app)
    {
        app.Services.AddCorsPolicy(app.Configuration);
        app.Services.AddResponseCache(app.Configuration);
        app.Services.AddErrorHandling();

        return app;
    }

    public static WebApplication UseShared(this WebApplication app)
    {
        app.UseCorsPolicy();
        app.UseResponseCaching();
        app.UseErrorHandling();

        return app;
    }
}
