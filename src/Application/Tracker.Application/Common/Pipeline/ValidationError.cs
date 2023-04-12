namespace Tracker.Application.Common.Pipeline;

public sealed record ValidationError(IEnumerable<string> Errors);