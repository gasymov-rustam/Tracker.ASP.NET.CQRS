using Tracker.Core.Entities;

namespace Tracker.Tests.Domain;

public class EmployeeDomainTest
{
    public Employee Employee = new Employee(
        "Test Employee",
        "man",
        new DateOnly(1990, 1, 1),
        Guid.NewGuid(),
        Guid.NewGuid()
        );

    [Fact]
    public void Update_Employee_SHOULD_BE_UPDATED()
    {
        // Arrange
        var newName = "New Test Employee";

        // Act
        var updatedEmployee = Employee.UpdateEmployeeName(Employee, newName);

        // Assert
        Assert.Equal(newName, updatedEmployee.Name);
        updatedEmployee.Name.Should().Be(newName);
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var name = "John";
        var sex = "Male";
        var birthday = new DateOnly(1990, 1, 1);
        var roleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var employee = new Employee(name, sex, birthday, roleId, userId);

        // Assert
        Assert.Equal(name, employee.Name);
        Assert.Equal(sex, employee.Sex);
        Assert.Equal(birthday, employee.Birthday);
        Assert.Equal(roleId, employee.RoleId);
        Assert.Equal(userId, employee.UserId);
    }

    //[Fact]
    //public void Update_Employee_SHOULD_BE_FAULT()
    //{
    //    // Arrange
    //    string newName = null;

    //    // Act
    //    var updatedEmployee = Employee.UpdateEmployeeName(Employee, newName);

    //    // Assert
    //    //Assert.Null(updatedEmployee);
    //    var ex = Assert.Throws<BaseException>(() => Employee.UpdateEmployeeName(updatedEmployee, newName));
    //    Assert.Equal(ExceptionCodes.ValueIsNullOrEmpty, ex.Code);
    //    //updatedEmployee.Should().Be(ExceptionCodes.ValueIsNullOrEmpty, "Name can not be empty!");
    //}
}

