using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Tracker.Shared.Observability.SwaggerAuth;

public static class SwaggerAuth
{
    public static IServiceCollection AddAuthSwagger(this IServiceCollection services)
    {
        _ = services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tracker Security", Version = "v1" });

            x.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                }
            );

            x.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                }
            );

            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //x.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
