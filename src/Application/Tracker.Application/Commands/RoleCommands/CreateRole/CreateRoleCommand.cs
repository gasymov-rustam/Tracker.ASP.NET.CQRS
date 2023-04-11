using Mediator;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.RoleCommands.CreateRoleCommand;

public record CreateRoleCommand(string Name) : ICommand<Guid>;

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