using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public sealed class Role : BaseEntity
{
    private readonly List<Employee> _employees = new();
    private readonly List<User> _users = new();
    public const string DefaultName = "employee";

    public Role(string name)
        : base(name) { }

    public IReadOnlyCollection<Employee> Employees => _employees;
    public IReadOnlyCollection<User> Users => _users;

    public static Role UpdateName(Role role, string name)
    {
        role.Name = name;

        return role;
    }
}
