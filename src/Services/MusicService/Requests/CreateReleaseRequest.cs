namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to create a <see cref="Models.Release"/>.
/// </summary>
/// 
/// <param name="Name">
///     The name of the <see cref="Models.Release"/>.
/// </param>
/// <param name="ReleaseTypeSlug">
///     The slug of the <see cref="Models.ReleaseType"/>.
/// </param>
/// <param name="ReleaseDate">
///     Release date as string. 
/// </param>
/// <param name="CoverUrl">
///     A URL to the cover image.
/// </param>
/// <param name="ArtistIds">
///     A collection of identifiers of artists participated in this release. 
/// </param>
/// <param name="Tracks">
///     Tracks of this release.
/// </param>
public sealed record CreateReleaseRequest(
    string Name,
    string ReleaseTypeSlug,
    string ReleaseDate,
    string CoverUrl,
    IEnumerable<Guid> ArtistIds,
    IEnumerable<CreateReleaseRequest.TrackInfo> Tracks
)
{
    /// <summary>
    ///     <see cref="Models.Track"/> information.
    /// </summary>
    /// <remarks>
    ///     Provide <paramref name="ArtistIds"/> only if it differ from <see cref="CreateReleaseRequest.ArtistIds"/>.
    /// </remarks>
    /// 
    /// <param name="Name">
    ///     The name of the track.
    /// </param>
    /// <param name="TagSlugs">
    ///     A collection of slugs of this track.
    /// </param>
    /// <param name="ArtistIds">
    ///     A collection of artists participated in creation of this track.
    /// </param>
    public sealed record TrackInfo(
        string Name,
        IEnumerable<string> TagSlugs,
        IEnumerable<Guid>? ArtistIds = null
    );
}