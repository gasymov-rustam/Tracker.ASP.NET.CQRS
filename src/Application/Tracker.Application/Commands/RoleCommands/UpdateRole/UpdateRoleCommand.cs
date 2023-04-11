using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.RoleCommands.UpdateRoleCommand;

public record UpdateRoleCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateRoleHandler : ICommandHandler<UpdateRoleCommand, Guid>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<UpdateRoleHandler> _logger;

    public UpdateRoleHandler(ITrackerDBContext context, ILogger<UpdateRoleHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask<Guid> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (role is null)
        {
            _logger.LogError("Role does not exist");
            return Guid.Empty;
        }

        var updatedRole = Role.UpdateName(role, command.Name);

        _context.Roles.Update(updatedRole);
        await _context.SaveChangesAsync(cancellationToken);

        return updatedRole.Id;
    }
}
