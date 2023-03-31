using Microsoft.EntityFrameworkCore;
using Tracker.Core.Entities;

namespace Tracker.Application.Common.Interfaces;

public interface ITrackerDBContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Activity> Activities { get; set; }
    Task<int> SaveChangesAsync(CancellationToken token);
}
