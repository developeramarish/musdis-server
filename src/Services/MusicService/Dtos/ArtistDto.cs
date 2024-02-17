using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="Artist"/>.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the <see cref="Artist"/>.
/// </param>
/// <param name="Name">
///     The name of the <see cref="Artist"/>.
/// </param>
/// <param name="Slug">
///     The slug of the <see cref="Artist"/>.
/// </param>
/// <param name="CoverUrl">
///     The cover image URL of the <see cref="Artist"/>.
/// </param>
/// <param name="CreatorId">
///     The identifier of user created the <see cref="Artist"/>.
/// </param>
/// <param name="Type">
///     The type of the <see cref="Artist"/>.
/// </param>
/// <param name="UserIds">
///     A collection of user identifiers that are participants of the <see cref="Artist"/>.
/// </param>
public sealed record ArtistDto(
    Guid Id,
    string Name,
    string Slug,
    string CoverUrl,
    string CreatorId,
    ArtistTypeDto Type,
    IEnumerable<string> UserIds
)
{
    /// <summary>
    ///     Maps an <see cref="Artist"/> to an <see cref="ArtistDto"/>.
    /// </summary>
    /// 
    /// <param name="artist">
    ///     The <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped <see cref="ArtistDto"/>.
    /// </returns>
    public static Result<ArtistDto> FromArtist(Artist artist)
    {
        if (artist?.ArtistType is null || artist?.ArtistUsers is null)
        {
            return Result<ArtistDto>.Failure(
                "Cannot convert Artist to ArtistDto, incorrect data passed."
            );
        }
    
        var artistTypeDtosResult = ArtistTypeDto.FromArtistType(artist.ArtistType);
        if (artistTypeDtosResult.IsFailure)
        {
            return Result<ArtistDto>.Failure(
                "Cannot convert Artist to ArtistDto, incorrect data passed."
            );
        }

        return new ArtistDto(
            artist.Id,
            artist.Name,
            artist.Slug,
            artist.CoverUrl,
            artist.CreatorId,
            artistTypeDtosResult.Value,
            artist.ArtistUsers.Select(au => au.UserId)
        ).ToValueResult();
    }

    /// <summary>
    ///     Maps a collection of <see cref="Artist"/> to a collection of <see cref="ArtistDto"/>.
    /// </summary>
    /// <param name="artists">
    ///     The collection of <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ArtistDto"/>.
    /// </returns>
    public static Result<IEnumerable<ArtistDto>> FromArtists(IEnumerable<Artist> artists)
    {
        List<ArtistDto> dtos = [];

        foreach (var artist in artists)
        {
            var result = FromArtist(artist);
            if (result.IsFailure)
            {
                return Result<IEnumerable<ArtistDto>>.Failure(
                    $"Cannot convert artists collection into artist DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }
}