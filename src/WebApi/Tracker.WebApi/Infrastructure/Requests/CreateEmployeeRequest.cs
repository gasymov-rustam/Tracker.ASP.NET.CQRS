namespace Tracker.WebApi.Infrastructure.Requests;

public record CreateEmployeeRequest(string Name, string Sex, DateOnly Birthday, Guid RoleId);