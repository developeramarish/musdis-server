namespace Musdis.MusicService.Requests;

/// <summary>
///     Represents file details for requests.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the file.
/// </param>
/// <param name="Url">
///     The URL of the file.
/// </param>
public record FileDetails(Guid Id, string Url);