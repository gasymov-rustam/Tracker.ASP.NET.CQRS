using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseQueryHandler;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.EmployeeQueries.GetAllEmployeesQuery;

public record GetEmployeeDto(Guid Id, string Name, string Sex, DateOnly Birthday, string RoleName);

public record GetAllEmployeesQuery : IQuery<IEnumerable<GetEmployeeDto>>;

public class GetAllEmployeesHandler : BaseQueryHandler<GetAllEmployeesQuery, IEnumerable<GetEmployeeDto>, ITrackerDBContext>
{
    public GetAllEmployeesHandler(ITrackerDBContext context, ILogger<GetAllEmployeesQuery> logger) : base(context, logger) { }

    public async override ValueTask<IEnumerable<GetEmployeeDto>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var allEmployees = await _context.Employees
                                        .AsTracking()
                                        .Select(x => new GetEmployeeDto(x.Id, x.Name, x.Sex, x.Birthday, x.Role.Name))
                                        .ToListAsync(cancellationToken);

        if (allEmployees is null)
        {
            _logger.LogError("GetAllEmployeesHandler: can not get all employees from database");
            return Enumerable.Empty<GetEmployeeDto>();
        }

        return allEmployees;
    }
}
