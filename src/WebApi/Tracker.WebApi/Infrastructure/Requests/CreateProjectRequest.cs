namespace Tracker.WebApi.Infrastructure.Requests;

public record CreateProjectRequest(string Name, DateOnly CreatedAt, DateOnly FinishedAt);
