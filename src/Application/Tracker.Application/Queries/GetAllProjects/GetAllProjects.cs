using Mediator;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.GetAllProjects;

public record GetAllProjectsDto(Guid Id, string Name, DateOnly CreatedAt, DateOnly FinishedAt);

public record GetAllProjectsQuery : IQuery<IQueryable<GetAllProjectsDto>>;

public class GetAllProjectsHandler : IQueryHandler<GetAllProjectsQuery, IQueryable<GetAllProjectsDto>>
{
  private readonly ITrackerDBContext _context;

  public GetAllProjectsHandler(ITrackerDBContext context) => _context = context;

  public ValueTask<IQueryable<GetAllProjectsDto>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
  {
    return ValueTask.FromResult(_context.Projects.Select(x => new GetAllProjectsDto(x.Id, x.Name, x.CreatedAt, x.FinishedAt)));
  }
}
