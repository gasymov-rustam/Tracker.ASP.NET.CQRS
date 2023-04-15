using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Common.BaseCommandHandler;
public abstract class BaseCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected readonly ITrackerDBContext _context;
    protected readonly ILogger<TCommand> _logger;
    protected readonly ICacheService _cacheService;
    public BaseCommandHandler(ITrackerDBContext context, ILogger<TCommand> logger, ICacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }

    public abstract ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}