namespace Tracker.Shared.ResponsesDto;

public record SecurityResponse(Guid UserId, string Name, string RoleName, string AccessToken, long Expiry);
