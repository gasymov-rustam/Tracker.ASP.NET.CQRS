namespace Tracker.WebApi.Infrastructure.Requests;

public record RegisterEmployeeRequest(string Name = default!, string Password = default!, Guid RoleId = default);
