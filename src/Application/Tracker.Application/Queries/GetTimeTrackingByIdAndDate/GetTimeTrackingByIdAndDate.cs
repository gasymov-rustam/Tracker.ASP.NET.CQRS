
using Mediator;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.GetTimeTrackingById;

public record GetTimeTrackingByIdAndDateDto(Guid EmployeeId, Guid ProjectId, DateOnly Start);

public record GetTimeTrackingByIdQuery(GetTimeTrackingByIdAndDateDto data) : IQuery<string>;

public class GetTimeTrackingByIdAndDateHandler : IQueryHandler<GetTimeTrackingByIdQuery, string>
{
  private readonly ITrackerDBContext _context;

  public GetTimeTrackingByIdAndDateHandler(ITrackerDBContext context) => _context = context;

  public async ValueTask<string> Handle(GetTimeTrackingByIdQuery query, CancellationToken cancellationToken)
  {
    var employee = await _context.Employees.FindAsync(query.data.EmployeeId, cancellationToken);
    var project = await _context.Projects.FindAsync(query.data.ProjectId, cancellationToken);

    if (employee is null || project is null)
      throw new Exception($"Employee with this id - {query.data.EmployeeId} does not exist");

    return $"Employee {employee.Name} is working on project {project.Name} since {query.data.Start}";
  }
}
