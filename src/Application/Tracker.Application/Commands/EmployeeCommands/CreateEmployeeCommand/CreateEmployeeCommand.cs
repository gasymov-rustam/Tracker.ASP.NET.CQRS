using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.EmployeeCommands.CreateEmployeeCommand;

public record CreateEmployeeCommand(string Name, string Sex, DateOnly Birthday, Guid RoleId) : ICommand<Guid>;

public class CreateEmployeeHandler : BaseCommandHandler<CreateEmployeeCommand, Guid, ITrackerDBContext>
{
    public CreateEmployeeHandler(ITrackerDBContext context, ILogger<CreateEmployeeCommand> logger) : base(context, logger) { }

    public async override ValueTask<Guid> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CreateEmployeeCommand");
        Employee employee = new(command.Name, command.Sex, command.Birthday, command.RoleId);
        var newEmployee = await _context.Employees.AddAsync(employee, cancellationToken);

        if (newEmployee.Entity is null)
        {
            _logger.LogError("CreateEmployeeCommand: can not add new employee");
            return Guid.Empty;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return newEmployee.Entity.Id;
    }
}