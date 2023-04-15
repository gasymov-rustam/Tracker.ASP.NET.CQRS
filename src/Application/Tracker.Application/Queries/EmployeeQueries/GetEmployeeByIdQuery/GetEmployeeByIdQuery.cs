using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseQueryHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Constants;
using Tracker.Application.Queries.EmployeeQueries.GetAllEmployeesQuery;
using Tracker.Core.Entities;

namespace Tracker.Application.Queries.EmployeeQueries.GetEmployeeByIdQuery;

public record GetEmployeeByIdQuery(Guid Id) : IQuery<GetEmployeeDto?>;

public class GetEmployeeByIdHandler : BaseQueryHandler<GetEmployeeByIdQuery, GetEmployeeDto?>
{
    public GetEmployeeByIdHandler(ITrackerDBContext context, ILogger<GetEmployeeByIdQuery> logger, ICacheService cacheService) : base(context, logger, cacheService) { }

    public async override ValueTask<GetEmployeeDto?> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var id = TrackerApplicationConsts.EMPLOYEE_REDIS_PREFIX + query.Id;

        return await _cacheService.GetAsync(id, async () =>
        {
            var employee = await _context.Employees.AsNoTracking()
                                                    .Include(x => x.Role)
                                                    .Select(x => new GetEmployeeDto(x.Id, x.Name, x.Sex, x.Birthday, x.Role.Name))
                                                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
            // .Where(x => x.Id == query.Id)
            // .Select(x => new GetEmployeeDto(x.Id, x.Name, x.Sex, x.Birthday, x.Role.Name))
            // .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
            {
                _logger.LogError("GetEmployeeByIdHandler: employee with id {Id} does not exist", query.Id);
                return null;
            }

            _logger.LogDebug(employee.ToString());

            return employee;
        }, cancellationToken);
    }
}