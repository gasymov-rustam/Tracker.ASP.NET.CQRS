namespace Tracker.Core.Entities;

public sealed class Activity
{
    public Guid ActivityId { get; private set; }
    public Guid ActivityTypeId { get; private set; }
    public Guid RoleId { get; private set; }
    public Guid ProjectId { get; private set; }
    public ActivityType ActivityType { get; set; }
    public Role Role { get; set; }
    public Project Project { get; set; }
    public Activity(Guid activityTypeId, Guid roleId, Guid projectId)
    {
        ActivityId = Guid.NewGuid();
        ActivityTypeId = activityTypeId;
        RoleId = roleId;
        ProjectId = projectId;
    }
}
