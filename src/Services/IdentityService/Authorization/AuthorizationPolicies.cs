namespace Musdis.IdentityService.Authorization;

/// <summary>
///     The authorization policies of the application.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    ///     The authorization policy for admins.
    /// </summary>
    public static string Admin { get; } = "Admin";
}