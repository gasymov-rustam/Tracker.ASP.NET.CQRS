using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseQueryHandler;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Queries.EmployeeQueries.GetAllEmployeesQuery;
using Tracker.Core.Entities;

namespace Tracker.Application.Queries.EmployeeQueries.GetEmployeeByIdQuery;

public record GetEmployeeByIdQuery(Guid Id) : IQuery<GetEmployeeDto>;

public class GetEmployeeByIdHandler : BaseQueryHandler<GetEmployeeByIdQuery, GetEmployeeDto, ITrackerDBContext>
{
    public GetEmployeeByIdHandler(ITrackerDBContext context, ILogger<GetEmployeeByIdQuery> logger) : base(context, logger) { }

    public async override ValueTask<GetEmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
                                        .Where(x => x.Id == query.Id)
                                        .Select(x => new GetEmployeeDto(x.Id, x.Name, x.Sex, x.Birthday, x.Role.Name))
                                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (employee is null)
        {
            _logger.LogError("GetEmployeeByIdHandler: employee with id {Id} does not exist", query.Id);
            return new GetEmployeeDto(Guid.Empty, string.Empty, string.Empty, default, string.Empty);
        }

        _logger.LogDebug(employee.ToString());

        return employee;
    }
}