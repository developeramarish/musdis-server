namespace Musdis.IdentityService.Requests;

/// <summary>
///     Represents file information.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the file.
/// </param>
/// <param name="Url">
///     The URL of the file.
/// </param>
public record FileDetails(
    Guid Id,
    string Url
);