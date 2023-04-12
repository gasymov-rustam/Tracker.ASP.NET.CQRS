using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Shared.Exceptions.Mappers;
using Tracker.Shared.Middleware;

namespace Tracker.Shared.Exceptions;
internal static class Extensions
{
    internal static IServiceCollection AddErrorHandling(this IServiceCollection services)
        => services
                .AddSingleton<ErrorHandlerMiddleware>()
                .AddSingleton<IExceptionMapper, ExceptionMapper>();

    internal static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandlerMiddleware>();
}