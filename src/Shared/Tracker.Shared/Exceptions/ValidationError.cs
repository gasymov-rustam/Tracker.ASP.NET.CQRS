namespace Tracker.Shared.Exceptions;

// public sealed record ValidationError(IDictionary<string> Errors);
public sealed class ValidationError
{
    public ValidationError(IReadOnlyDictionary<string, string[]> errorsDictionary)
        => ValidationErrors = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; }
}