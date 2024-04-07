using Musdis.MusicService.Models;

namespace Musdis.MusicService.Requests;

/// <summary>
///     A request to update the <see cref="Track"/>.
/// </summary>
/// 
/// <param name="Title">
///     A new title of the <see cref="Track"/>.
/// </param>
/// <param name="ReleaseId">
///     An identifier of the new <see cref="Release"/> to which the track is attached.
/// </param>
/// <param name="AudioFile">
///     A new file of the <see cref="Track"/>
/// </param>
/// <param name="ArtistIds">
///     An updated collection of artists created this <see cref="Track"/>.
/// </param>
/// <param name="TagSlugs">
///     An updated list of tags of this <see cref="Track"/>.
/// </param>
public sealed record UpdateTrackRequest(
    string? Title,
    Guid? ReleaseId,
    FileDetails? AudioFile,
    IEnumerable<Guid>? ArtistIds,
    IEnumerable<string>? TagSlugs
);
