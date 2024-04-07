using Musdis.AuthHelpers.Options;

namespace Musdis.IdentityService.Options;

/// <summary>
///     Represents JWT configuration section.
/// </summary>
public sealed class JwtConfigurationOptions
{
    /// <summary>
    ///     The name of the JWT configuration section.
    /// </summary>
    public const string Jwt = "Jwt";

    /// <summary>
    ///     The JWT security options.
    /// </summary>
    public required JwtOptions Security { get; init; }

    /// <summary>
    ///     The JWT settings.
    /// </summary>
    public required JwtSettingsOptions Settings { get; init; }
}