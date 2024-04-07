namespace Musdis.IdentityService.Options;

/// <summary>
///     Represents Identity password configuration section.
/// </summary>
public sealed class IdentityPasswordOptions
{
    /// <summary>
    ///     The name of the Identity password configuration section.
    /// </summary>
    public const string Password = "Identity:Password";

    /// <summary>
    ///     Is the password required to contain at least one digit.
    /// </summary>
    public required bool RequireDigit { get; init; }

    /// <summary>
    ///     Is the password required to contain at least one non alphanumeric character.
    /// </summary>
    public required bool RequireNonAlphanumeric { get; init; }

    /// <summary>
    ///     Is the password required to contain at least one lowercase letter.
    /// </summary>
    public required bool RequireLowercase { get; init; }

    /// <summary>
    ///     Is the password required to contain at least one uppercase letter.
    /// </summary>
    public required bool RequireUppercase { get; init; }

    /// <summary>
    ///     The minimum length of the password.
    /// </summary>
    public required int RequiredLength { get; init; }

    /// <summary>
    ///     The minimum number of unique characters in the password.
    /// </summary>
    public required int RequiredUniqueChars { get; init; }
}