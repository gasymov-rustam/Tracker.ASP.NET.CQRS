namespace Tracker.Core.Primitive;
public interface IBaseEntity
{
    Guid Id { get; }
    string Name { get; }
}