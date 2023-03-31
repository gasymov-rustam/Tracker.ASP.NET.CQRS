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
    var newProject = _context.Projects.Add(new Project(command.project.Name, command.project.CreatedAt, command.project.FinishedAt));
    await _context.SaveChangesAsync(cancellationToken);

    return newProject.Entity.Id;
  }
}
