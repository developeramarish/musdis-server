namespace Musdis.AuthHelpers.Options;

/// <summary>
///     Represents JWT configuration section.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    ///     The JWT issuer.
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    ///     The JWT audience.
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    ///     The JWT signing key.
    /// </summary>
    public required string Key { get; init; }
}