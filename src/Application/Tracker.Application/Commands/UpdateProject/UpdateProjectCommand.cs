using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.UpdateProject;


public record UpdateProjectCommand(string Id, string Name, DateOnly CreatedAt, DateOnly FinishedAt) : ICommand<Guid>;

public class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand, Guid>
{
    private readonly ITrackerDBContext _context;
    private readonly ILogger<UpdateProjectHandler> _logger;

    public UpdateProjectHandler(ITrackerDBContext context, ILogger<UpdateProjectHandler> logger) { _context = context; _logger = logger; }

    public async ValueTask<Guid> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(command.Id);

        _logger.LogInformation($"Update project with id - {id}");

        var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null)
            throw new Exception($"Project with this id - {command.Id} does not exist");

        var updatedProject = Project.UpdateProject(project, command.Name, command.CreatedAt, command.FinishedAt);

        _context.Projects.Update(updatedProject);

        await _context.SaveChangesAsync(cancellationToken);

        return updatedProject.Id;
    }
}
