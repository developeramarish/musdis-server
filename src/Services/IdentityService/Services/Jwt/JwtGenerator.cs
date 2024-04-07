using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Musdis.IdentityService.Requests;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Musdis.IdentityService.Options;

namespace Musdis.IdentityService.Services.Jwt;

/// <inheritdoc cref="IJwtGenerator"/>
public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfigurationOptions _jwtConfigurationOptions;
    private readonly TimeProvider _timeProvider;
    private readonly SymmetricSecurityKey _securityKey;

    public JwtGenerator(IOptions<JwtConfigurationOptions> options, TimeProvider timeProvider)
    {
        _jwtConfigurationOptions = options.Value;
        _timeProvider = timeProvider;

        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurationOptions.Security.Key));
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
            .AddMinutes(_jwtConfigurationOptions.Settings.TokenLifetimeMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtConfigurationOptions.Security.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = localExpires.ToUniversalTime(),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
