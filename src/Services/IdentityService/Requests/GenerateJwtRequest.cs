using System.Security.Claims;

using Musdis.IdentityService.Dtos;

namespace Musdis.IdentityService.Requests;

/// <summary>
///     Represents a request for generating a JSON Web Token (JWT) with optional custom claims.
/// </summary>
/// 
/// <param name="UserReadDto">
///     The user data for generating the JWT.
/// </param>
/// <param name="CustomClaims">
///     A collection of custom claims to include in the JWT.
/// </param>
public record GenerateJwtRequest(
    UserReadDto UserReadDto,
    IEnumerable<Claim> CustomClaims
);