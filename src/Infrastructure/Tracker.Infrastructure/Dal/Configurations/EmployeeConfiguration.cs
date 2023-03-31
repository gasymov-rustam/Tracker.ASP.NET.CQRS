using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Core.Entities;

namespace Tracker.Infrastructure.Dal.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Sex).IsRequired();
        builder.Property(e => e.Birthday).IsRequired();
        builder.HasOne(e => e.Role).WithMany(e => e.Employees).HasForeignKey(e => e.RoleId);
    }
}
