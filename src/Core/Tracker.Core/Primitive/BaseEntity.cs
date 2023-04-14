namespace Tracker.Core.Primitive;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; }
    public string Name { get; protected set; } = string.Empty;
    public BaseEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
