using Mediator;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.GetSingleProjectById;

public record GetSingleProjectDto(Guid Id, string Name, DateOnly CreatedAt, DateOnly FinishedAt);

public record GetSingleProjectByIdQuery(Guid Id) : IQuery<GetSingleProjectDto>;

public class GetSingleProjectByIdHandler : IQueryHandler<GetSingleProjectByIdQuery, GetSingleProjectDto>
{
  private readonly ITrackerDBContext _context;

  public GetSingleProjectByIdHandler(ITrackerDBContext context) => _context = context;

  public async ValueTask<GetSingleProjectDto> Handle(GetSingleProjectByIdQuery query, CancellationToken cancellationToken)
  {
    var project = await _context.Projects.FindAsync(query.Id, cancellationToken);

    if (project is null)
      throw new Exception($"Project with this id - {query.Id} does not exist");

    return new GetSingleProjectDto(project.Id, project.Name, project.CreatedAt, project.FinishedAt);
  }
}
