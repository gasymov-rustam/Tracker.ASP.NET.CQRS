namespace Tracker.Core.Primitive;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; protected set; } = string.Empty;
    public BaseEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
