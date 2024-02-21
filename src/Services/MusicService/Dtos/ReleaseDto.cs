using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for a release.
/// </summary>
///     
/// <param name="Id">
///     the unique identifier of the release.
/// </param>
/// <param name="ReleaseType">
///     the release type information for the release.
/// </param>
/// <param name="Name">
///     the name of the release.
/// </param>
/// <param name="Slug">
///     the slug of the release.
/// </param>
/// <param name="ReleaseDate">
///     the release date of the release.
/// </param>
/// <param name="CoverUrl">
///     the cover URL of the release.
/// </param>
/// <param name="Artists">
///     the collection of artists associated with the release.
/// </param>
/// <param name="Tracks">
///     the collection of track information associated with the release.
/// </param>
public sealed record ReleaseDto(
    Guid Id,
    ReleaseTypeDto ReleaseType,
    string Name,
    string Slug,
    string ReleaseDate,
    string CoverUrl,
    IEnumerable<ArtistDto> Artists,
    IEnumerable<ReleaseDto.TrackInfo> Tracks
)
{
    /// <summary>
    ///     Maps a <see cref="Release"/> to a <see cref="ReleaseDto"/>.
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
    ///     The mapped <see cref="ReleaseDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public static ReleaseDto FromRelease(Release release)
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
            ArtistDto.FromArtists(release.Artists),
            TrackInfo.FromTracks(release.Tracks)
        );
    }
    /// <summary>
    ///     Maps a collection of <see cref="Release"/> to a collection of <see cref="ReleaseDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure every release's <see cref="Release.ReleaseType"/>, 
    ///     <see cref="Release.Artists"/>, <see cref="Release.Tracks"/> are not null.
    /// </remarks>
    /// 
    /// <param name="releases">
    ///     The collection of <see cref="Release"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped collection of <see cref="ReleaseDto"/>.
    /// </returns>  
    public static IEnumerable<ReleaseDto> FromReleases(IEnumerable<Release> releases)
    {
        return releases.Select(r => FromRelease(r));
    }

    /// <summary>
    ///     Represents track information within a release.
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
    /// <param name="Tags">
    ///     The collection of tags associated with the track.
    /// </param>
    /// <param name="Artists">
    ///     The collection of artists associated with the track.
    /// </param>
    public sealed record TrackInfo(
        Guid Id,
        string Title,
        string Slug,
        IEnumerable<TagDto> Tags,
        IEnumerable<ArtistDto> Artists
    )
    {
        /// <summary>
        ///     Maps a <see cref="Track"/> to a <see cref="TrackInfo"/>.
        /// </summary>
        /// <remarks>
        ///     Make sure <paramref name="track"/>'s <see cref="Track.Artists"/>, 
        ///     <see cref="Track.Tags"/> are not null.
        /// </remarks>
        /// 
        /// <param name="track">
        ///     The <see cref="Track"/> to map.
        /// </param>
        /// 
        /// <returns>
        ///     The mapped <see cref="TrackInfo"/>.
        /// </returns>
        /// <exception cref="InvalidMethodCallException">
        ///     Thrown if method called incorrectly, see remarks.
        /// </exception>
        private static TrackInfo FromTrack(Track track)
        {
            if (track?.Artists is null || track.Tags is null)
            {
                throw new InvalidMethodCallException(
                    "Cannot convert track into track info, make sure it is or its properties are not null."
                );
            }

            return new(
                track.Id,
                track.Title,
                track.Slug,
                TagDto.FromTags(track.Tags),
                ArtistDto.FromArtists(track.Artists)
            );
        }

        /// <summary>
        ///     Maps a collection of <see cref="Track"/> to a collection of <see cref="TrackInfo"/>.
        /// </summary>
        /// <remarks>
        ///      Make sure every track's <see cref="Track.Artists"/>, 
        ///     <see cref="Track.Tags"/>, are not null.
        /// </remarks>
        /// 
        /// <param name="tracks">
        ///     The collection of <see cref="Track"/> to map
        /// </param>
        /// <returns>
        ///     The mapped collection of <see cref="TrackInfo"/>.
        /// </returns>
        public static IEnumerable<TrackInfo> FromTracks(
            IEnumerable<Track> tracks
        )
        {
            return tracks.Select(t => FromTrack(t));
        }
    }
}

