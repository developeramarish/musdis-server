namespace Musdis.IdentityService.Options;

public class IdentityPasswordOptions
{
    public const string Password = "Identity:Password";

    public bool RequireDigit { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
}