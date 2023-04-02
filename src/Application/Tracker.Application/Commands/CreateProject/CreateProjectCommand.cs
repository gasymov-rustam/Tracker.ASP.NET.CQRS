using Mapster;
using Mediator;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Application.Commands.CreateProject;

public record CreateProjectDto(string Name, DateOnly CreatedAt, DateOnly FinishedAt);

public record CreateProjectCommand(CreateProjectDto project) : ICommand<Guid>;

public class CreateProjectHandler : ICommandHandler<CreateProjectCommand, Guid>
{
  private readonly ITrackerDBContext _context;

  public CreateProjectHandler(ITrackerDBContext context)
  {
    _context = context;
  }

  public async ValueTask<Guid> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
  {
    var project = command.project.Adapt<Project>();
    var newProject = _context.Projects.Add(project);

    await _context.SaveChangesAsync(cancellationToken);

    if (newProject is null)
      return Guid.Empty;

    return newProject.Entity.Id;
  }
}
