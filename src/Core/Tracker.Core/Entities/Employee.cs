﻿using Tracker.Core.Primitive;
using Tracker.Shared.Exceptions;

namespace Tracker.Core.Entities;

public sealed class Employee : BaseEntity
{
    private readonly List<Project> _projects = new();
    public string Sex { get; }
    public DateOnly Birthday { get; }
    public IReadOnlyCollection<Project> Projects => _projects;
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Employee(string name, string sex, DateOnly birthday, Guid roleId, Guid userId)
        : base(name)
    {
        Sex = sex;
        Birthday = birthday;
        RoleId = roleId;
        UserId = userId;
    }

    public static Employee UpdateEmployeeName(Employee employee, string Name)
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty, "Name can not be empty!");
        }
        else
        {
            employee.Name = Name;

            return employee;
        }
    }

    //public override string ToString()
    //{
    //    return $"Project: {Name} - {Sex} - {Role.Id} - {Role.Name} - {RoleId}";
    //}
}
