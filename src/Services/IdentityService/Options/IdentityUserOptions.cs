namespace Musdis.IdentityService.Options;

// TODO add docs
public sealed class IdentityUserOptions
{
    public const string User = "Identity:User";

    public bool RequireUniqueEmail { get; init; }

    public required string AllowedUserNameCharacters { get; init; } 

    public int MaxUserNameLength { get; init; }

    public int MinUserNameLength { get; init; }
}