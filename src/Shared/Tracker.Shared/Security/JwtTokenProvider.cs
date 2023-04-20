using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tracker.Shared.Security.Time;

namespace Tracker.Shared.Security;

public interface IJwtTokenProvider
{
    JsonWebToken CreateToken(string userId, string role, IDictionary<string, string>? claims);
}

public sealed class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IUtcClock _clock;
    private readonly JwtOptions _jwtOptions;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public JwtTokenProvider(IUtcClock clock, JwtOptions jwtOptions, IJwtTokenGenerator tokenGenerator)
    {
        _clock = clock;
        _jwtOptions = jwtOptions;
        _tokenGenerator = tokenGenerator;
    }

    public JsonWebToken CreateToken(string userId, string role, IDictionary<string, string>? claims)
    {
        var now = _clock.GetCurrentUtc();

        var jwtClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
        };

        if (!string.IsNullOrWhiteSpace(role))
            jwtClaims.Add(new Claim(ClaimTypes.Role, role));

        // if (!string.IsNullOrWhiteSpace(firstName))
        //     jwtClaims.Add(new Claim(ClaimTypes.Name, firstName));

        // if (!string.IsNullOrWhiteSpace(lastName))
        //     jwtClaims.Add(new Claim(ClaimTypes.Surname, lastName));

        var expires = now.AddMinutes(_jwtOptions.ExpiryMinutes);

        var jwt = _tokenGenerator.GenerateToken(
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            expires,
            jwtClaims
        );

        return new JsonWebToken(jwt, expires.ToTimestamp(), userId, role ?? string.Empty, jwtClaims);
    }
}
