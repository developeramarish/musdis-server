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
    TrackDto.ReleaseInfo Release,
    IEnumerable<TagDto> Tags,
    IEnumerable<ArtistDto> Artists
)
{
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
    );

    public static Result<TrackDto> FromTrack(Track track)
    {
        if (track?.Artists is null || track?.Tags is null || track?.Release is null)
        {
            return Result<TrackDto>.Failure(
                "Cannot convert Track to Track DTO: Track or related data is null."
            );
        }

        var tagsResult = TagDto.FromTags(track.Tags);
        if (tagsResult.IsFailure)
        {
            return Result<TrackDto>.Failure(
                $"Cannot convert Track to Track DTO: {tagsResult.Error.Description}"
            );
        }

        var artistsResult = ArtistDto.FromArtists(track.Artists);
        if (artistsResult.IsFailure)
        {
            return Result<TrackDto>.Failure(
                $"Cannot convert Track to Track DTO: {artistsResult.Error.Description}"
            );
        }

        var releaseInfoResult = ConvertReleaseToInfo(track.Release);
        if (releaseInfoResult.IsFailure)
        {
            return Result<TrackDto>.Failure(
                $"Cannot convert Track to TrackDto: {releaseInfoResult.Error.Description}"
            );
        }

        return new TrackDto(
            track.Id,
            track.Title,
            track.Slug,
            releaseInfoResult.Value,
            tagsResult.Value,
            artistsResult.Value
        ).ToValueResult();
    }

    /// <summary>
    ///     Maps a collection of <see cref="Track"/> to a collection of <see cref="TrackDto"/>.
    /// </summary>
    /// 
    /// <param name="tracks">
    ///     The collection of <see cref="Track"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result{T}"/> containing the mapped collection of <see cref="TrackDto"/>.
    /// </returns>
    public static Result<IEnumerable<TrackDto>> FromTracks(IEnumerable<Track> tracks)
    {
        List<TrackDto> dtos = [];

        foreach (var track in tracks)
        {
            var result = FromTrack(track);
            if (result.IsFailure)
            {
                return Result<IEnumerable<TrackDto>>.Failure(
                    $"Cannot convert tag collection into tag DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }

    private static Result<ReleaseInfo> ConvertReleaseToInfo(Release release)
    {
        if (release?.ReleaseType is null || release?.Artists is null)
        {
            return Result<ReleaseInfo>.Failure(
                "Cannot convert Release to TrackDto.ReleaseInfo: provided value or related data is null"
            );
        }

        var releaseTypeResult = ReleaseTypeDto.FromReleaseType(release.ReleaseType);
        if (releaseTypeResult.IsFailure)
        {
            return Result<ReleaseInfo>.Failure(
                $"Cannot convert Release to TrackDto.ReleaseInfo: {releaseTypeResult.Error.Description}"
            );
        }

        var artistsResult = ArtistDto.FromArtists(release.Artists);
        if (artistsResult.IsFailure)
        {
            return Result<ReleaseInfo>.Failure(
                $"Cannot convert Release to TrackDto.ReleaseInfo: {artistsResult.Error.Description}"
            );
        }

        return new ReleaseInfo(
            release.Id,
            releaseTypeResult.Value,
            release.Name,
            release.Slug,
            release.ReleaseDate.ToString("o"),
            release.CoverUrl,
            artistsResult.Value
        ).ToValueResult();
    }
}