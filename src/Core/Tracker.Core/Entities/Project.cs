using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;
public sealed class Project : BaseEntity
{
    public DateOnly CreatedAt { get; private set; }
    public DateOnly FinishedAt { get; private set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public Project(string name) : base(name) { }

    public Project(string name, DateOnly finishedAt, DateOnly startDate, Guid employeeId) : base(name)
    {
        CreatedAt = startDate;
        FinishedAt = finishedAt;
        EmployeeId = employeeId;
    }

    public static Project UpdateFinishDate(Project project, DateOnly date)
    {
        project.FinishedAt = date;

        return project;
    }

    public static Project UpdateName(Project project, string name)
    {
        project.Name = name;

        return project;
    }

    public static Project UpdateProject(Project project, string name, DateOnly date, DateOnly startDate)
    {
        project.Name = name;
        project.FinishedAt = date;
        project.CreatedAt = startDate;

        return project;
    }

    public override string ToString()
    {
        return $"Project: {Name} - {CreatedAt} - {FinishedAt}";
    }
}
