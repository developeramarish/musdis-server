namespace Musdis.MusicService.Requests;

/// <summary>
///     A request to create a <see cref="Models.Track"/>.
/// </summary>
/// 
/// <param name="Title">
///     The title of the <see cref="Models.Track"/>.
/// </param>
/// <param name="ReleaseId">
///     The identifier of the <see cref="Models.Release"/> that 
///     the <see cref="Models.Track"/> being created is the part of.
/// </param>
/// <param name="AudioFile">
///     The file of the <see cref="Models.Track"/>.
/// </param>
/// <param name="ArtistIds">
///     Identifiers of artists, who are the creators of the track.
/// </param>
/// <param name="TagSlugs">
///     Tags related to the track being created.
/// </param>
public sealed record CreateTrackRequest(
    string Title,
    Guid ReleaseId,
    FileDetails AudioFile,
    IEnumerable<Guid> ArtistIds,
    IEnumerable<string> TagSlugs
);