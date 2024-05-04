namespace Musdis.ApiGateway.Responses;

/// <summary>
///     Represents a user.
/// </summary>
/// 
/// <param name="Id">
///     The unique identifier of the user.
/// </param>
/// <param name="UserName">
///     The user's username.
/// </param>
/// <param name="AvatarUrl">
///     The link to the avatar file of the authenticated user.
/// </param>
/// <param name="Email">
///     The user's email address.
/// </param>
public sealed record User(
    string Id,
    string UserName,
    string AvatarUrl,
    string Email
);
