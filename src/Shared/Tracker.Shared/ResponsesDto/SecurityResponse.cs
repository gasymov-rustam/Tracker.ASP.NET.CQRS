namespace Tracker.Shared.ResponsesDto;

public record SecurityResponse(Guid UserGid, string Name, string AccessToken, long Expiry);
