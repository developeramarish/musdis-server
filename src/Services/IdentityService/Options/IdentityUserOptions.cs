namespace Musdis.IdentityService.Options;

/// <summary>
///     Represents Identity user configuration section.
/// </summary>
public sealed class IdentityUserOptions
{
    /// <summary>
    ///     The name of the Identity user configuration section.
    /// </summary>
    public const string User = "Identity:User";

    /// <summary>
    ///     Is the user required to have unique email.
    /// </summary>
    public required bool RequireUniqueEmail { get; init; }

    /// <summary>
    ///     The allowed characters in the user name.
    /// </summary>
    public required string AllowedUserNameCharacters { get; init; } 

    /// <summary>
    ///     The maximum length of the user name.
    /// </summary>
    public required int MaxUserNameLength { get; init; }

    /// <summary>
    ///     The minimum length of the user name.
    /// </summary>
    public required int MinUserNameLength { get; init; }
}