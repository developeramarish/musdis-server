using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Musdis.IdentityService.Services.Jwt;

/// <inheritdoc cref="IJwtGenerator"/>
public class JwtGenerator : IJwtGenerator
{
    private readonly JwtOptions _jwtOptions;
    private readonly TimeProvider _timeProvider;
    private readonly SymmetricSecurityKey _securityKey;

    public JwtGenerator(IOptions<JwtOptions> options, TimeProvider timeProvider)
    {
        _jwtOptions = options.Value;
        _timeProvider = timeProvider;

        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
    }

    /// <inheritdoc cref="IJwtGenerator.GenerateToken(GenerateJwtRequest)"/>
    /// <exception cref="ArgumentNullException"> </exception>
    public string GenerateToken(GenerateJwtRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, request.UserReadDto.Id),
            new (JwtRegisteredClaimNames.Name, request.UserReadDto.UserName),
        };
        claims.AddRange(request.CustomClaims);

        var localExpires = _timeProvider.GetLocalNow().DateTime
            .AddMinutes(_jwtOptions.TokenLifetimeMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = localExpires.ToUniversalTime(),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
