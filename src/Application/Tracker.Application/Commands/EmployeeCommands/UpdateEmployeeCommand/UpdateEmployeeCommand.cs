using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Constants;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.EmployeeCommands.UpdateEmployeeCommand;

public record UpdateEmployeeCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateEmployeeHandler : BaseCommandHandler<UpdateEmployeeCommand, Guid, ITrackerDBContext>
{
    public UpdateEmployeeHandler(ITrackerDBContext context, ILogger<UpdateEmployeeCommand> logger, ICacheService cacheService) : base(context, logger, cacheService) { }

    public async override ValueTask<Guid> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee? currentEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (currentEmployee is null)
        {
            _logger.LogError("UpdateEmployeeCommand: employee with id {Id} does not exist", command.Id);
            return Guid.Empty;
        }

        Employee employee = Employee.UpdateEmployeeName(currentEmployee, command.Name);
        var updatedEmployee = _context.Employees.Update(employee);

        if (updatedEmployee.Entity is null)
        {
            _logger.LogError("UpdateEmployeeCommand: can not update employee with id {Id}", command.Id);
            return Guid.Empty;
        }

        await _context.SaveChangesAsync(cancellationToken);

        var id = TrackerApplicationConsts.EMPLOYEE_REDIS_PREFIX + updatedEmployee.Entity.Id;

        await _cacheService.SetAsync(id, updatedEmployee.Entity, cancellationToken);

        return updatedEmployee.Entity.Id;
    }
}
