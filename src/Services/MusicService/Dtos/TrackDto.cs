using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a data transfer object for a track.
/// </summary>
/// 
/// <param name="Id">
///     The unique identifier of the track.
/// </param>
/// <param name="Title">
///     The title of the track.
/// </param>
/// <param name="Slug">
///     The slug of the track.
/// </param>
/// <param name="AudioUrl">
///     The audio URL of the track.
/// </param>
/// <param name="CoverUrl">
///     The URL of the cover image.
/// </param>
/// <param name="Release">
///     The release information for the track.
/// </param>
/// <param name="Tags">
///     The collection of tags associated with the track.
/// </param>
/// <param name="Artists">
///     The collection of artists associated with the track.
/// </param>
public sealed record TrackDto(
    Guid Id,
    string Title,
    string Slug,
    string AudioUrl,
    string CoverUrl,
    TrackDto.ReleaseInfo Release,
    IEnumerable<TagDto> Tags,
    IEnumerable<ArtistDto> Artists
)
{
    /// <summary>
    ///     Maps a <see cref="Track"/> to a <see cref="TrackDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure <paramref name="track"/>'s <see cref="Track.Artists"/>, 
    ///     <see cref="Track.Tags"/>, <see cref="Track.Release"/> are not null.
    /// </remarks>
    /// 
    /// <param name="track">
    ///     The <see cref="Track"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped <see cref="TrackDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public static TrackDto FromTrack(Track track)
    {
        if (track?.Artists is null || track.Tags is null || track.Release is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert track into DTO, make sure it is not null."
            );
        }

        return new(
            track.Id,
            track.Title,
            track.Slug,
            track.AudioUrl,
            track.CoverUrl,
            ReleaseInfo.FromRelease(track.Release),
            TagDto.FromTags(track.Tags),
            ArtistDto.FromArtists(track.Artists)
        );
    }

    /// <summary>
    ///     Maps a collection of <see cref="Track"/> to a collection of <see cref="TrackDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure every track's <see cref="Track.Artists"/>, 
    ///     <see cref="Track.Tags"/>, <see cref="Track.Release"/> are not null.
    /// </remarks>
    /// 
    /// <param name="tracks">
    ///     The collection of <see cref="Track"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result{T}"/> containing the mapped collection of <see cref="TrackDto"/>.
    /// </returns>
    public static IEnumerable<TrackDto> FromTracks(IEnumerable<Track> tracks)
    {
        return tracks.Select(t => FromTrack(t));
    }

    /// <summary>
    ///     Represents release information for a track.
    /// </summary>
    /// 
    /// <param name="Id">
    ///     The unique identifier of the release.
    /// </param>
    /// <param name="ReleaseType">
    ///     The release type information for the release.
    /// </param>
    /// <param name="Name">
    ///     The name of the release.
    /// </param>
    /// <param name="Slug">
    ///     The slug of the release.
    /// </param>
    /// <param name="ReleaseDate">
    ///     The release date of the release.
    /// </param>
    /// <param name="CoverUrl">
    ///     The cover URL of the release.
    /// </param>
    /// <param name="Artists">
    ///     The collection of artists associated with the release.
    /// </param>
    public sealed record ReleaseInfo(
        Guid Id,
        ReleaseTypeDto ReleaseType,
        string Name,
        string Slug,
        string ReleaseDate,
        string CoverUrl,
        IEnumerable<ArtistDto> Artists
    )
    {
        /// <summary>
        ///     Maps a <see cref="Release"/> to a <see cref="ReleaseInfo"/>.
        /// </summary>
        /// <remarks>
        ///     Make sure <paramref name="release"/>'s <see cref="Release.ReleaseType"/>, 
        ///     <see cref="Release.Artists"/>, <see cref="Release.Tracks"/> are not null.
        /// </remarks>
        /// 
        /// <param name="release">
        ///     The <see cref="Release"/> to map.
        /// </param>
        /// 
        /// <returns>
        ///     The mapped <see cref="ReleaseInfo"/>.
        /// </returns>
        /// <exception cref="InvalidMethodCallException">
        ///     Thrown if method called incorrectly, see remarks.
        /// </exception>
        public static ReleaseInfo FromRelease(Release release)
        {
            if (release?.ReleaseType is null || release.Artists is null || release.Tracks is null)
            {
                throw new InvalidMethodCallException(
                    "Cannot convert release to DTO: check if provided value or related data is not null."
                );
            }

            return new(
                release.Id,
                ReleaseTypeDto.FromReleaseType(release.ReleaseType),
                release.Name,
                release.Slug,
                release.ReleaseDate.ToString("o"),
                release.CoverUrl,
                ArtistDto.FromArtists(release.Artists)
            );
        }
    }
}