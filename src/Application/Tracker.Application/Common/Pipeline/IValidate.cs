using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace Tracker.Application.Common.Pipeline;

public interface IValidate : IMessage
{
    bool IsValid([NotNullWhen(false)] out ValidationError? error);
}