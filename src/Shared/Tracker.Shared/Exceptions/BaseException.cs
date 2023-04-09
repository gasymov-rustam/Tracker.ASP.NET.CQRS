namespace Tracker.Shared.Exceptions;
public sealed class BaseException : Exception
{
    public string Code { get; set; } = string.Empty;

    public BaseException() { }

    public BaseException(string code)
        => Code = code;

    public BaseException(string message, params object[] args)
        : this(string.Empty, message, args) { }

    public BaseException(string code, string message, params object[] args)
        : this(null, code, message, args) { }

    public BaseException(Exception innerException, string message, params object[] args)
        : this(innerException, string.Empty, message, args) { }

    public BaseException(Exception innerException, string code, string message, params object[] args)
        : base(string.Format(message, args), innerException) { Code = code; }
}