namespace Musdis.IdentityService.Dtos;

/// <summary>
///     Represents a DTO for an authenticated user with JWT and additional claims.
/// </summary>
/// 
/// <param name="User"> 
///     The user associated with the authentication.
/// </param>
/// <param name="Jwt"> 
///     The JSON Web Token associated with the authentication. 
/// </param>
/// <param name="AdditionalClaims"> 
///     The additional claims associated with the authenticated user. 
/// </param>
public record AuthenticatedUserDto(
    UserDto User,
    string Jwt,
    Dictionary<string, string> AdditionalClaims
);