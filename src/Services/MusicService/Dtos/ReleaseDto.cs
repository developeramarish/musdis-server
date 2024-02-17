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
    /// 
    /// <param name="release">
    ///     The <see cref="Release"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped <see cref="ReleaseDto"/>.
    /// </returns>
    public static Result<ReleaseDto> FromRelease(Release release)
    {
        if (release?.ReleaseType is null || release?.Artists is null || release?.Tracks is null)
        {
            return Result<ReleaseDto>.Failure(
                "Cannot convert Release to ReleaseDto: provided value or related data is null"
            );
        }

        var releaseTypeResult = ReleaseTypeDto.FromReleaseType(release.ReleaseType);
        if (releaseTypeResult.IsFailure)
        {
            return Result<ReleaseDto>.Failure(
                $"Cannot convert Release to ReleaseDto: {releaseTypeResult.Error.Description}"
            );
        }

        var artistsResult = ArtistDto.FromArtists(release.Artists);
        if (artistsResult.IsFailure)
        {
            return Result<ReleaseDto>.Failure(
                $"Cannot convert Release to ReleaseDto: {artistsResult.Error.Description}"
            );
        }

        var tracksResult = TrackInfo.FromTracks(release.Tracks);
        if (tracksResult.IsFailure)
        {
            return Result<ReleaseDto>.Failure(
                $"Cannot convert Release to ReleaseDto: {tracksResult.Error.Description}"
            );
        }


        return new ReleaseDto(
            release.Id,
            releaseTypeResult.Value,
            release.Name,
            release.Slug,
            release.ReleaseDate.ToString("o"),
            release.CoverUrl,
            artistsResult.Value,
            null!
        ).ToValueResult();
    }
    /// <summary>
    ///     Maps a collection of <see cref="Release"/> to a collection of <see cref="ReleaseDto"/>.
    /// </summary>
    /// 
    /// <param name="releases">
    ///     The collection of <see cref="Release"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ReleaseDto"/>.
    /// </returns>  
    public static Result<IEnumerable<ReleaseDto>> FromReleases(IEnumerable<Release> releases)
    {
        List<ReleaseDto> dtos = [];

        foreach (var release in releases)
        {
            var result = FromRelease(release);
            if (result.IsFailure)
            {
                return Result<IEnumerable<ReleaseDto>>.Failure(
                    $"Cannot convert tag collection into tag DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
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
        /// 
        /// <param name="track">
        ///     The <see cref="Track"/> to map.
        /// </param>
        /// 
        /// <returns>
        ///     A <see cref="Result"/> containing the mapped <see cref="TrackInfo"/>.
        /// </returns>
        private static Result<TrackInfo> FromTrack(Track track)
        {
            if (track?.Artists is null || track?.Tags is null || track?.Release is null)
            {
                return Result<TrackInfo>.Failure(
                    "Cannot convert Track to TrackInfo: Track or related data is null."
                );
            }

            var tagsResult = TagDto.FromTags(track.Tags);
            if (tagsResult.IsFailure)
            {
                return Result<TrackInfo>.Failure(
                    $"Cannot convert Track to TrackInfo: {tagsResult.Error.Description}"
                );
            }

            var artistsResult = ArtistDto.FromArtists(track.Artists);
            if (artistsResult.IsFailure)
            {
                return Result<TrackInfo>.Failure(
                    $"Cannot convert Track to TrackInfo: {artistsResult.Error.Description}"
                );
            }

            return new TrackInfo(
                track.Id,
                track.Title,
                track.Slug,
                tagsResult.Value,
                artistsResult.Value
            ).ToValueResult();
        }

        /// <summary>
        ///     Maps a collection of <see cref="Track"/> to a collection of <see cref="TrackInfo"/>.
        /// </summary>
        /// 
        /// <param name="tracks">
        ///     The collection of <see cref="Track"/> to map
        /// </param>
        /// <returns>
        ///     A <see cref="Result"/> containing the mapped collection of <see cref="TrackInfo"/>.
        /// </returns>
        public static Result<IEnumerable<TrackInfo>> FromTracks(
            IEnumerable<Track> tracks
        )
        {
            List<TrackInfo> dtos = [];

            foreach (var track in tracks)
            {
                var result = FromTrack(track);
                if (result.IsFailure)
                {
                    return Result<IEnumerable<TrackInfo>>.Failure(
                        $"Cannot convert tag collection into tag DTOs: {result.Error.Description}"
                    );
                }

                dtos.Add(result.Value);
            }

            return dtos.AsEnumerable().ToValueResult();
        }
    }
}

