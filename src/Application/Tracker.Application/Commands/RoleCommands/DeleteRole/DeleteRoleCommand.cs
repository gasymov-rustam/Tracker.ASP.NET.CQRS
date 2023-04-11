using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Commands.RoleCommands.DeleteRoleCommand;

public record DeleteRoleCommand(Guid Id) : ICommand<Guid>;

public class DeleteRoleHandler : ICommandHandler<DeleteRoleCommand, Guid>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<DeleteRoleHandler> _logger;

    public DeleteRoleHandler(ITrackerDBContext context, ILogger<DeleteRoleHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask<Guid> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (role is null)
        {
            _logger.LogError("Role does not exist");
            return Guid.Empty;
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        return role.Id;
    }
}