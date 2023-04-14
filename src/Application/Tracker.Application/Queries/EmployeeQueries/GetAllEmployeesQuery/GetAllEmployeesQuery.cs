using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseQueryHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Primitive;

namespace Tracker.Application.Queries.EmployeeQueries.GetAllEmployeesQuery;

public record GetEmployeeDto(Guid Id, string Name, string Sex, DateOnly Birthday, string RoleName) : IBaseEntity;

public record GetAllEmployeesQuery : IQuery<IEnumerable<GetEmployeeDto>>;

public class GetAllEmployeesHandler : BaseQueryHandler<GetAllEmployeesQuery, IEnumerable<GetEmployeeDto>, ITrackerDBContext>
{
    public GetAllEmployeesHandler(ITrackerDBContext context, ILogger<GetAllEmployeesQuery> logger, ICacheService cacheService) : base(context, logger, cacheService) { }

    public async override ValueTask<IEnumerable<GetEmployeeDto>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var cachedEmployees = await _cacheService.GetAllAsync(async () =>
        {
            return await _context.Employees
                                       .AsTracking()
                                       .Select(x => new GetEmployeeDto(x.Id, x.Name, x.Sex, x.Birthday, x.Role.Name))
                                       .ToListAsync(cancellationToken);
        }, cancellationToken);

        return cachedEmployees;
    }
}
