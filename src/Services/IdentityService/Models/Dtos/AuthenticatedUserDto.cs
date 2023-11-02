using System.Security.Claims;

namespace IdentityService.Models.Dtos;

/// <summary>
/// Represents a DTO for an authenticated user with JWT and additional claims.
/// </summary>
/// <param name="Id"> The unique identifier of the authenticated user. </param>
/// <param name="UserName"> The username of the authenticated user. </param>
/// <param name="Email"> The email address of the authenticated user. </param>
/// <param name="Jwt"> The JSON Web Token associated with the authentication. </param>
/// <param name="AdditionalClaims"> The additional claims associated with the authenticated user. </param>
public record AuthenticatedUserDto(
    string Id,
    string UserName,
    string Email,
    string Jwt,
    IEnumerable<KeyValuePair<string, string>> AdditionalClaims
);