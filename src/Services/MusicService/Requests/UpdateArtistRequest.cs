namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to update <see cref="Models.Artist"/> entity.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of an <see cref="Models.Artist"/>.
/// </param>
/// <param name="Name">
///     A new name.
/// </param>
/// <param name="ArtistTypeSlug">
///     A slug of changed <see cref="Models.ArtistType"/>.
/// </param>
/// <param name="CoverUrl">
///     A new URL to the cover image of the <see cref="Models.Artist"/>.
/// </param>
/// <param name="UserIds">
///     A new collection of user identifiers related with this <see cref="Models.Artist"/>.
/// </param>
public record UpdateArtistRequest(
    Guid Id,
    string? Name,
    string? ArtistTypeSlug,
    string? CoverUrl,
    IEnumerable<string>? UserIds
);