using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public sealed class Employee : BaseEntity
{
    private readonly List<Project> _projects = new();
    public string Sex { get; private set; }
    public DateOnly Birthday { get; private set; }
    public IReadOnlyCollection<Project> Projects => _projects;
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public Employee(string name, string sex, DateOnly birthday, Guid roleId) : base(name)
    {
        Sex = sex;
        Birthday = birthday;
        RoleId = roleId;
    }
}
