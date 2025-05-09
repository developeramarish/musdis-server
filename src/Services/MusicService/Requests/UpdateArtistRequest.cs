namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to update <see cref="Models.Artist"/> entity.
/// </summary>
/// 
/// <param name="Name">
///     A new name.
/// </param>
/// <param name="ArtistTypeSlug">
///     A slug of changed <see cref="Models.ArtistType"/>.
/// </param>
/// <param name="CoverFile">
///     A new file of <see cref="Models.Artist"/>
/// </param>
/// <param name="UserIds">
///     A new collection of user identifiers related with this <see cref="Models.Artist"/>.
/// </param>
public sealed record UpdateArtistRequest(
    string? Name,
    string? ArtistTypeSlug,
    FileDetails? CoverFile,
    IEnumerable<string>? UserIds
);