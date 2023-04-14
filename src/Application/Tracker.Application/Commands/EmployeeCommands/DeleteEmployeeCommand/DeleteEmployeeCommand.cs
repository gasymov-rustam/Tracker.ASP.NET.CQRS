using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.EmployeeCommands.DeleteEmployeeCommand;

public record DeleteEmployeeCommand(Guid Id) : ICommand<Guid>;

public class DeleteEmployeeHandler : BaseCommandHandler<DeleteEmployeeCommand, Guid, ITrackerDBContext>
{
    public DeleteEmployeeHandler(ITrackerDBContext context, ILogger<DeleteEmployeeCommand> logger, ICacheService cacheService) : base(context, logger, cacheService) { }

    public async override ValueTask<Guid> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (employee is null)
        {
            _logger.LogError("DeleteEmployeeCommand: employee with id {Id} does not exist", command.Id);
            return Guid.Empty;
        }

        var deletedEmployee = _context.Employees.Remove(employee);

        if (deletedEmployee.Entity is null)
        {
            _logger.LogError("DeleteEmployeeCommand: can not delete employee with id {Id}", command.Id);
            return Guid.Empty;
        }

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync(deletedEmployee.Entity.Id.ToString(), cancellationToken);

        return deletedEmployee.Entity.Id;
    }
}