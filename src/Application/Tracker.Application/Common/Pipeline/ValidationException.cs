namespace Tracker.Application.Common.Pipeline;
public sealed class ValidationException : Exception
{
    public ValidationException(ValidationError validationError) : base("Validation error") =>
        ValidationError = validationError;

    public ValidationError ValidationError { get; }
}