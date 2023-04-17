using System.Security.Claims;

namespace Tracker.Shared.Security;

public sealed class JsonWebToken
{
    public string AccessToken { get; } = default!;

    public long Expires { get; }

    public string UserGid { get; } = default!;

    public string Role { get; } = default!;

    public IEnumerable<Claim>? Claims { get; }
    public JsonWebToken(string accessToken, long expires, string userGid, string role, IEnumerable<Claim>? claims)
    {
        AccessToken = accessToken;
        Expires = expires;
        UserGid = userGid;
        Role = role;
        Claims = claims;
    }
}