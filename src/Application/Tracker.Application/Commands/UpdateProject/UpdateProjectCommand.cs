using Mediator;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.UpdateProject;

public record UpdateProjectDto(string Name, DateOnly CreatedAt, DateOnly FinishedAt);

public record UpdateProjectCommand(Guid Id, UpdateProjectDto project) : ICommand<Guid>;

public class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand, Guid>
{
  private readonly ITrackerDBContext _context;

  public UpdateProjectHandler(ITrackerDBContext context) => _context = context;

  public async ValueTask<Guid> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
  {
    var project = await _context.Projects.FindAsync(command.Id, cancellationToken);

    if (project is null)
      throw new Exception($"Project with this id - {command.Id} does not exist");

    var updatedProject = Project.UpdateProject(project, command.project.Name, command.project.CreatedAt, command.project.FinishedAt);

    _context.Projects.Update(updatedProject);

    await _context.SaveChangesAsync(cancellationToken);

    return updatedProject.Id;
  }
}
