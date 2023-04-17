using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tracker.Shared.Constants;
using Tracker.Shared.Security.Time;

namespace Tracker.Shared.Security;

internal static class Extensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.BindOptions<JwtOptions>(TrackerOptionsConsts.JwtOptionSectionName);

        services.AddSingleton(jwtOptions);

        services.AddSingleton<IUtcClock, UtcClock>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IStorage, HttpStorage>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                SecurityKey issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

                jwt.SaveToken = true;

                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = issuerSigningKey,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

        services.AddAuthorization();
        // services.AddAuthorization(options =>
        // {
        //     if (jwtOptions.Roles.Any())
        //         foreach (var role in jwtOptions.Roles)
        //             options.AddPolicy(role.Name, policy => policy.RequireRole(role.Name));
        // });

        return services;
    }

    public static long ToTimestamp(this DateTime date)
    {
        var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var expectedDate = date.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }
}