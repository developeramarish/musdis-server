namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to generate an <see cref="Models.Artist"/>.
/// </summary>
/// 
/// <param name="Name">
///     The name of <see cref="Models.Artist"/>.
/// </param>
/// <param name="ArtistTypeSlug">
///     The slug of <see cref="Models.ArtistType"/> of creating <see cref="Models.Artist"/>.
/// </param>
/// <param name="CoverFile">
///     The cover image of <see cref="Models.Artist"/>.
/// </param>
/// <param name="UserIds">
///     Identifiers of users who are participants of an artist (e.g. members of band).
/// </param>
public sealed record CreateArtistRequest(
    string Name,
    string ArtistTypeSlug,
    FileDetails CoverFile,
    IEnumerable<string> UserIds
);