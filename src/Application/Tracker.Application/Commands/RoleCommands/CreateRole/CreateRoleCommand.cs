using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Common.Pipeline;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.RoleCommands.CreateRoleCommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).Length(3, 15).NotEmpty().WithErrorCode("409").WithMessage("Name is required");
    }
}

public record CreateRoleCommand(string Name) : ICommand<Guid>, IValidate
{
    public bool IsValid([NotNullWhen(false)] out ValidationError? error)
    {
        var validator = new CreateRoleCommandValidator();
        var result = validator.Validate(this);

        if (result.IsValid)
            error = null;
        else
            error = new ValidationError(result.Errors.Select(e => e.ErrorMessage).ToArray());

        return result.IsValid;
    }
};

public class CreateRoleHandler : ICommandHandler<CreateRoleCommand, Guid>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<CreateRoleHandler> _logger;

    public CreateRoleHandler(ITrackerDBContext context, ILogger<CreateRoleHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask<Guid> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = new Role(command.Name);

        var newRole = await _context.Roles.AddAsync(role, cancellationToken);

        if (newRole is null)
        {
            _logger.LogError("Role does not created");
            return Guid.Empty;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return newRole.Entity.Id;
    }
}