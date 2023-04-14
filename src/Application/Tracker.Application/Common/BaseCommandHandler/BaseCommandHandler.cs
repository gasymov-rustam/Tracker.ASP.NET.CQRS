using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Caching;

namespace Tracker.Application.Common.BaseCommandHandler;
public abstract class BaseCommandHandler<TCommand, TResult, DbContext> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected readonly DbContext _context;
    protected readonly ILogger<TCommand> _logger;
    protected readonly ICacheService _cacheService;
    public BaseCommandHandler(DbContext context, ILogger<TCommand> logger, ICacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }

    public abstract ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}