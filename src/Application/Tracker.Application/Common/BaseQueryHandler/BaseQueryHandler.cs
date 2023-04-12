using Mediator;
using Microsoft.Extensions.Logging;

namespace Tracker.Application.Common.BaseQueryHandler;

public abstract class BaseQueryHandler<TQuery, TResult, DbContext> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected readonly DbContext _context;
    protected readonly ILogger<TQuery> _logger;
    public BaseQueryHandler(DbContext context, ILogger<TQuery> logger)
    {
        _context = context;
        _logger = logger;
    }
    public abstract ValueTask<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}