namespace Musdis.IdentityService.Defaults;

/// <summary>
/// Contains error types for problem details definition.
/// </summary>
public static class ProblemDetailsErrorTypes
{
    public static string InternalError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
    public static string NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
}