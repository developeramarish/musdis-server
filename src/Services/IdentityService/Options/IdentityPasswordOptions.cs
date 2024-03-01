namespace Musdis.IdentityService.Options;

// TODO add docs
public sealed class IdentityPasswordOptions
{
    public const string Password = "Identity:Password";

    public bool RequireDigit { get; init; }
    public bool RequireNonAlphanumeric { get; init; }
    public bool RequireLowercase { get; init; }
    public bool RequireUppercase { get; init; }
    public int RequiredLength { get; init; }
    public int RequiredUniqueChars { get; init; }
}