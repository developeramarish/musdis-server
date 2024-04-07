namespace Musdis.IdentityService.Options;

/// <summary>
///     Represents Identity configuration section.
/// </summary>
public sealed class IdentityConfigOptions
{
    /// <summary>
    ///     The name of the Identity configuration section.
    /// </summary>
    public const string Identity = "Identity";

    /// <summary>
    ///     The password options.
    /// </summary>
    public required IdentityPasswordOptions Password { get; init; }

    /// <summary>
    ///     The user options.
    /// </summary>
    public required IdentityUserOptions User { get; init; }
}