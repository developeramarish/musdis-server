namespace Musdis.IdentityService.Options;

// TODO add docs
public sealed class IdentityConfigOptions
{
    public const string Identity = "Identity";

    public required IdentityPasswordOptions Password { get; init; }

    public required IdentityUserOptions User { get; init; }
}