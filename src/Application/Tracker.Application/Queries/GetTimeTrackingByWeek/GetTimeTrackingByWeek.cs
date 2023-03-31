using System.Globalization;
using Mediator;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.GetTimeTrackingByWeek;

public record GetTimeTrackingByWeekDto(Guid EmployeeId, int NumberOfWeek);

public record GetTimeTrackingByWeekQuery(GetTimeTrackingByWeekDto data) : IQuery<string>;

public class GetTimeTrackingByWeekHandler : IQueryHandler<GetTimeTrackingByWeekQuery, string>
{
  private readonly ITrackerDBContext _context;

  public GetTimeTrackingByWeekHandler(ITrackerDBContext context) => _context = context;

  public ValueTask<string> Handle(GetTimeTrackingByWeekQuery query, CancellationToken cancellationToken)
  {
    var projects = _context.Projects.Where(x => x.EmployeeId == query.data.EmployeeId).ToList();
    CultureInfo cul = CultureInfo.CurrentCulture;

    if (projects is null)
      throw new Exception($"Employee with this id - {query.data.EmployeeId} does not exist");

    foreach (var item in projects)
    {
      var d = new DateTime(item.CreatedAt.Year, item.CreatedAt.Month, item.CreatedAt.Day);

      var firstDayWeek = cul.Calendar.GetWeekOfYear(
        d,
        CalendarWeekRule.FirstDay,
        DayOfWeek.Monday);

      int weekNum = cul.Calendar.GetWeekOfYear(
      d,
      CalendarWeekRule.FirstDay,
      DayOfWeek.Monday);

      if (weekNum == query.data.NumberOfWeek)
      {
        return ValueTask.FromResult($"Employee {item.Employee.Name} is working on project {item.Name} since {item.CreatedAt}");
      }
    }

    return ValueTask.FromResult("No projects found");
  }
}