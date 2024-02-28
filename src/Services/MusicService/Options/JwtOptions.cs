namespace Musdis.MusicService.Options;

// TODO add docs
public sealed class JwtOptions
{
    public const string Jwt = "JwtSettings";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int TokenLifetimeMinutes { get; set; }
}