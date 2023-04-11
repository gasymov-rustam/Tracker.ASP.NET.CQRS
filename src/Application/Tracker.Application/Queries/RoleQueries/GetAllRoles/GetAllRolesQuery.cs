using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Queries.RoleQueries.GetAllRoles;

public record RoleDto(Guid Id, string Name);

public record GetAllRolesQuery : IQuery<IQueryable<RoleDto>>;

public class GetAllRolesHandler : IQueryHandler<GetAllRolesQuery, IQueryable<RoleDto>>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<GetAllRolesHandler> _logger;

    public GetAllRolesHandler(ITrackerDBContext context, ILogger<GetAllRolesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public ValueTask<IQueryable<RoleDto>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_context.Roles.AsNoTracking().Select(x => new RoleDto(x.Id, x.Name)));
    }
}
