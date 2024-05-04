namespace Musdis.ApiGateway.Responses;

/// <summary>
///     Response of an authentication method calls.
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
public sealed record AuthenticatedUser(
    User User,
    string Jwt,
    Dictionary<string, string> AdditionalClaims
);