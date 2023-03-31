using Mediator;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand<Guid>;

public class DeleteProjectHandler : ICommandHandler<DeleteProjectCommand, Guid>
{
  private readonly ITrackerDBContext _context;

  public DeleteProjectHandler(ITrackerDBContext context) => _context = context;

  public async ValueTask<Guid> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
  {
    var project = await _context.Projects.FindAsync(command.Id, cancellationToken);

    if (project is null)
      throw new Exception($"Project with this id - {command.Id} does not exist");

    _context.Projects.Remove(project);

    await _context.SaveChangesAsync(cancellationToken);

    return project.Id;
  }
}
