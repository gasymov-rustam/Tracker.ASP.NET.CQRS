using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Common.BaseQueryHandler;

public abstract class BaseQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected readonly ITrackerDBContext _context;
    protected readonly ILogger<TQuery> _logger;
    protected readonly ICacheService _cacheService;
    public BaseQueryHandler(ITrackerDBContext context, ILogger<TQuery> logger, ICacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }
    public abstract ValueTask<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}