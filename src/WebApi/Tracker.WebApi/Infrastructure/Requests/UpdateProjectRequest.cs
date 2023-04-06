namespace Tracker.WebApi.Infrastructure.Requests;

public record UpdateProjectRequest(string Name, DateOnly CreatedAt, DateOnly FinishedAt)