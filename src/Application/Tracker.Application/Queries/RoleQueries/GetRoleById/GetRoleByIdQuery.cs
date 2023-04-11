using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.RoleQueries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<Guid>;

public class GetRGetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, Guid>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<GetRGetRoleByIdQueryHandler> _logger;

    public GetRGetRoleByIdQueryHandler(ITrackerDBContext context, ILogger<GetRGetRoleByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask<Guid> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (role is null)
        {
            _logger.LogError("Role does not exist");
            return Guid.Empty;
        }

        return role.Id;
    }
}
