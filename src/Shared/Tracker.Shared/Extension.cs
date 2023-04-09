using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Shared.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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

        return app;
    }

    public static WebApplication UseShared(this WebApplication app)
    {
        app.UseCorsPolicy();

        return app;
    }
}
