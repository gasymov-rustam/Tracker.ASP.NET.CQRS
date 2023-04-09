using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tracker.Shared.Cors;

internal static class Extensions
{
    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.BindOptions<CorsOptions>("Cors");
        services.AddSingleton(options);

        return services.AddCors(cors =>
        {
            var allowedHeaders = options.AllowedHeaders;
            var allowedMethods = options.AllowedMethods;
            var allowedOrigins = options.AllowedOrigins;

            cors.AddPolicy("Cors", corsPolicy =>
            {
                var hosts = allowedOrigins?.ToArray() ?? Array.Empty<String>();

                if (options.AllowCredentials && hosts.FirstOrDefault() != "*")
                    corsPolicy.AllowCredentials();
                else
                    corsPolicy.DisallowCredentials();

                corsPolicy
                    .WithHeaders(allowedHeaders?.ToArray() ?? Array.Empty<string>())
                    .WithMethods(allowedMethods?.ToArray() ?? Array.Empty<string>())
                    .WithOrigins(hosts.ToArray());
            });
        });
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors("Cors");
}