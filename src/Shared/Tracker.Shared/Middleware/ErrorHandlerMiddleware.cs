using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Pipeline;
using Tracker.Shared.Exceptions.Mappers;

namespace Tracker.Shared.Middleware;
internal sealed class ErrorHandlerMiddleware : IMiddleware
{
    private readonly IExceptionMapper _exceptionMapper;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(
        IExceptionMapper exceptionMapper,
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _exceptionMapper = exceptionMapper ?? throw new ArgumentNullException(nameof(exceptionMapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        if (exception is ValidationException exc)
        {
            context.Response.StatusCode = HttpStatusCode.Conflict.GetHashCode();
            await context.Response.WriteAsJsonAsync(exc.ValidationError);

            return;
        }

        var errorResponse = _exceptionMapper.Map(exception);

        context.Response.StatusCode = (int)(errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);

        var response = errorResponse?.Response;

        if (response is null) return;

        await context.Response.WriteAsJsonAsync(response);
    }
}