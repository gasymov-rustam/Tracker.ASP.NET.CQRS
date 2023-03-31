using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public sealed class Role : BaseEntity
{
    private readonly List<Employee> _emplyees = new();
    public Role(string name) : base(name) { }
    public IReadOnlyCollection<Employee> Employees => _emplyees;
    public static Role UpdateName(Role role, string name)
    {
        role.Name = name;

        return role;
    }
}
