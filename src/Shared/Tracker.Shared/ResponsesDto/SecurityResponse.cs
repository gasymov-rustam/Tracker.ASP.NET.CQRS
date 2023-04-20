namespace Tracker.Shared.ResponsesDto;

public record SecurityResponse(Guid UserId, string Name, string AccessToken, long Expiry);
