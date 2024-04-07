namespace Musdis.IdentityService.Options;

/// <summary>
///     Represents JWT settings configuration section.
/// </summary>
public sealed class JwtSettingsOptions
{
    /// <summary>
    ///     The token lifetime in minutes.
    /// </summary>
    public required int TokenLifetimeMinutes { get; init; }
}