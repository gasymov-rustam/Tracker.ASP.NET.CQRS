using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public class User : BaseEntity
{
    public string Password { get; } = string.Empty;
    public Guid RoleId { get; } = Guid.Empty;
    public Role Role { get; }

    public User(string name, string password, Guid roleId)
        : base(name)
    {
        RoleId = roleId;
        Password = password;
    }
}
