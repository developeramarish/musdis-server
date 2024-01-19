using Musdis.IdentityService.Models.Dtos;
using Musdis.IdentityService.Models.Requests;

namespace Musdis.IdentityService.Services.JwtGenerator;

/// <summary> 
/// Service for generating JWTs based on user data.
/// </summary>
public interface IJwtGenerator
{
    /// <summary>
    /// Generates a JWT based on the provided user data.
    /// </summary>
    /// <param name="request">The request used to create the JWT.</param>
    /// <returns>The generated JWT as a string.</returns>
    string GenerateToken(GenerateJwtRequest request);
}
