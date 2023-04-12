using Mediator;
using Microsoft.Extensions.Logging;

namespace Tracker.Application.Common.BaseCommandHandler;
public abstract class BaseCommandHandler<TCommand, TResult, DbContext> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected readonly DbContext _context;
    protected readonly ILogger<TCommand> _logger;
    public BaseCommandHandler(DbContext context, ILogger<TCommand> logger)
    {
        _context = context;
        _logger = logger;
    }
    public abstract ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}