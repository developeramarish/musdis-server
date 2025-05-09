namespace Musdis.MusicService.Requests;

/// <summary>
///     A request to update a release.
/// </summary>
/// 
/// <param name="Name">
///     A new name of the release.
/// </param>
/// <param name="ReleaseTypeSlug">
///     A slug of new release type.
/// </param>
/// <param name="ReleaseDate">
///     An updated release date.
/// </param>
/// <param name="CoverFile">
///     A new file of cover of the release.
/// </param>
/// <param name="ArtistIds">
///     An updated collection of artists of the release.
/// </param>
public sealed record UpdateReleaseRequest(
    string? Name,
    string? ReleaseTypeSlug,
    string? ReleaseDate,
    FileDetails? CoverFile,
    IEnumerable<Guid>? ArtistIds
);