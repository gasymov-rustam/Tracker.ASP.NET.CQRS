using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Caching;

namespace Tracker.Application.Common.BaseQueryHandler;

public abstract class BaseQueryHandler<TQuery, TResult, DbContext> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected readonly DbContext _context;
    protected readonly ILogger<TQuery> _logger;
    protected readonly ICacheService _cacheService;
    public BaseQueryHandler(DbContext context, ILogger<TQuery> logger, ICacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }
    public abstract ValueTask<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}