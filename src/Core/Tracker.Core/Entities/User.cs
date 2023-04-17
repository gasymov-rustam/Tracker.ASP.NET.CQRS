using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public class User : BaseEntity
{
    public string Password { get; } = string.Empty;
    public User(string name, string password) : base(name)
    {
        Password = password;
    }
}