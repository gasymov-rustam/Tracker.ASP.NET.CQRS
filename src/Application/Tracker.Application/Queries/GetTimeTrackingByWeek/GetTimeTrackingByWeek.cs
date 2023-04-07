using Mediator;
using System.Globalization;
using Tracker.Application.Common.Interfaces;

namespace Tracker.Application.Queries.GetTimeTrackingByWeek;

public record GetTimeTrackingByWeekQuery(Guid EmployeeId, int NumberOfWeek) : IQuery<string>;

public class GetTimeTrackingByWeekHandler : IQueryHandler<GetTimeTrackingByWeekQuery, string>
{
    private readonly ITrackerDBContext _context;

    public GetTimeTrackingByWeekHandler(ITrackerDBContext context) => _context = context;

    public ValueTask<string> Handle(GetTimeTrackingByWeekQuery query, CancellationToken cancellationToken)
    {
        var projects = _context.Projects.Where(x => x.EmployeeId == query.EmployeeId).ToList();
        CultureInfo cul = CultureInfo.CurrentCulture;

        if (projects is null)
            throw new Exception($"Employee with this id - {query.EmployeeId} does not exist");

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

            if (weekNum == query.NumberOfWeek)
            {
                var employee = _context.Employees.FirstOrDefault(x => x.Id == query.EmployeeId);

                if (employee is null)
                    return ValueTask.FromResult($"Employee with this id - {query.EmployeeId} does not exist");


                return ValueTask.FromResult($"Employee {employee.Name} is working on project {item.Name} since {item.CreatedAt}");
            }
        }

        return ValueTask.FromResult("No projects found");
    }
}