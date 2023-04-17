using Microsoft.EntityFrameworkCore;
using Tracker.Application.Common.Interfaces;
using Tracker.Core.Entities;

namespace Tracker.Infrastructure.Dal;

public class TrackerDbContext : DbContext, ITrackerDBContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<User> Users { get; set; }
    public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackerDbContext).Assembly);
    }
}
