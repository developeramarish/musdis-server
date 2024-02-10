using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="ArtistType"/> reading.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the <see cref="ArtistType"/>.
/// </param>
/// <param name="Name">
///     The name of the <see cref="ArtistType"/>.
/// </param>
/// <param name="Slug">
///     The slug of the <see cref="ArtistType"/>.
/// </param>
public sealed record ArtistTypeDto(
    Guid Id,
    string Name,
    string Slug
)
{
    /// <summary>
    ///     Creates <see cref="ArtistTypeDto"/> from <see cref="ArtistType"/>.
    /// </summary>
    /// 
    /// <param name="artistType">
    ///     The <see cref="ArtistType"/> to generate DTO from.
    /// </param>
    /// 
    /// <returns>
    ///     The result object that contains a <see cref="ArtistTypeDto"/> value 
    ///     which is a converted <see cref="ArtistType"/>.
    /// </returns>
    public static Result<ArtistTypeDto> FromArtistType(ArtistType artistType)
    {
        if (artistType is null)
        {
            return new InternalServerError(
                "Cannot convert ArtistType to ArtistTypeDto, value is null"
            ).ToValueResult<ArtistTypeDto>();
        }

        return new ArtistTypeDto(
            artistType.Id, 
            artistType.Name, 
            artistType.Slug
        ).ToValueResult();
    }
}