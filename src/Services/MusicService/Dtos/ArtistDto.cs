using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="Artist"/> reading.
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
    ///     Creates <see cref="ArtistDto"/> from <see cref="Artist"/>.
    /// </summary>
    /// 
    /// <param name="artist">
    ///     An <see cref="Artist"/> to convert.
    /// </param>
    /// 
    /// <returns>
    ///     The result object that contains a <see cref="ArtistDto"/> value 
    ///     which is a converted <see cref="Artist"/>.
    /// </returns>
    public static Result<ArtistDto> FromArtist(Artist artist)
    {
        if (artist?.ArtistType is null || artist?.ArtistUsers is null)
        {
            return new InternalServerError(
                "Cannot convert Artist to ArtistDto, incorrect data passed."
            ).ToValueResult<ArtistDto>();
        }
    
        var artistTypeDtosResult = ArtistTypeDto.FromArtistType(artist.ArtistType);
        if (artistTypeDtosResult.IsFailure)
        {
            return new InternalServerError(
                "Cannot convert Artist to ArtistDto, incorrect data passed."
            ).ToValueResult<ArtistDto>();
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
    ///     Converts <see cref="Artist"/> collection into <see cref="ArtistDto"/> collection.
    /// </summary>
    /// 
    /// <param name="artists">
    ///     A collection to convert.
    /// </param>
    /// 
    /// <returns>
    ///     The result object that contains an <see cref="IEnumerable{T}"/> 
    ///     of <see cref="ArtistDto"/> value which is a converted <see cref="Artist"/>s.
    /// </returns>
    public static Result<IEnumerable<ArtistDto>> FromArtists(IEnumerable<Artist> artists)
    {
        List<ArtistDto> dtos = [];

        foreach (var artist in artists)
        {
            var result = FromArtist(artist);
            if (result.IsFailure)
            {
                return new InternalServerError(
                    "Cannot convert artists collection into artist DTOs"
                ).ToValueResult<IEnumerable<ArtistDto>>();
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }
}