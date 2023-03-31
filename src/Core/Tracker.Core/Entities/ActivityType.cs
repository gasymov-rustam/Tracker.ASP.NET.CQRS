using Tracker.Core.Primitive;

namespace Tracker.Core.Entities;

public sealed class ActivityType : BaseEntity
{
    public ActivityType(string name) : base(name) { }
    public static ActivityType UpdateName(ActivityType activityType, string name)
    {
        activityType.Name = name;

        return activityType;
    }
}
