using System.Net;

namespace Tracker.Shared.Exceptions;

internal sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);
