using System.ComponentModel.DataAnnotations;
using System.Collections.Concurrent;
using System.Net;
using Humanizer;

namespace Tracker.Shared.Exceptions.Mappers;

internal interface IExceptionMapper
{
    ExceptionResponse Map(Exception exception);
}

internal sealed class ExceptionMapper : IExceptionMapper
{
    private readonly ExceptionResponse _defaultResponse = new(
        new ErrorsResponse(new Error("default_error", "There was an error.")), HttpStatusCode.InternalServerError);

    private readonly ExceptionResponse _communicationResponse = new(
        new ErrorsResponse(new Error("http_communication_error", "There was an HTTP error.")), HttpStatusCode.InternalServerError);

    private readonly ConcurrentDictionary<Type, string> _codes = new();

    public ExceptionResponse Map(Exception exception)
    {
        if (exception is ValidationException exc)
        {
            Console.WriteLine(exc.Message);
        }
        return exception switch
        {
            BaseException => new ExceptionResponse(new Error(GetErrorCode(exception), exception.Message), HttpStatusCode.BadRequest),
            HttpRequestException => _communicationResponse,
            ValidationException => new ExceptionResponse(new Error("validation_error", exception.Message), HttpStatusCode.BadRequest),
            _ => _defaultResponse
        };
    }

    private string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return _codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }
}

public record Error(string Code, string Message);

public record ErrorsResponse(params Error[] Errors);