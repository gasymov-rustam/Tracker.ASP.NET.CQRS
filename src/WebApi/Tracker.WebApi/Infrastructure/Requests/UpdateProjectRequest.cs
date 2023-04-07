namespace Tracker.WebApi.Infrastructure.Requests;

public record UpdateProjectRequest(string Id, string Name, DateOnly CreatedAt, DateOnly FinishedAt);