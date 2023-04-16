using System.Net;

namespace Tracker.Shared.Exceptions;

public sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);