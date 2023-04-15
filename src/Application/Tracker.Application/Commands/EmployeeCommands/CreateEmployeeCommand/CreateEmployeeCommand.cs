using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Constants;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.EmployeeCommands.CreateEmployeeCommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).Length(3, 15).NotEmpty().WithErrorCode("409").WithMessage("Employee Name is required");
        RuleFor(x => x.Sex).NotEmpty().WithMessage("Please choose");
    }
}

public record CreateEmployeeCommand(string Name, string Sex, DateOnly Birthday, Guid RoleId) : ICommand<Guid>;

public class CreateEmployeeHandler : BaseCommandHandler<CreateEmployeeCommand, Guid>
{
    public CreateEmployeeHandler(ITrackerDBContext context, ILogger<CreateEmployeeCommand> logger, ICacheService cacheService) : base(context, logger, cacheService) { }

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

        var id = TrackerApplicationConsts.EMPLOYEE_REDIS_PREFIX + newEmployee.Entity.Id;

        await _cacheService.SetAsync(id, newEmployee.Entity, cancellationToken);

        return newEmployee.Entity.Id;
    }
}