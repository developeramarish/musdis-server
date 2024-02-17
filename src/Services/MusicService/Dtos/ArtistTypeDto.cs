using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="ArtistType"/>.
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
    ///     Maps an <see cref="ArtistType"/> to an <see cref="ArtistTypeDto"/>.
    /// </summary>
    /// 
    /// <param name="artistType">
    ///     The <see cref="ArtistType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped <see cref="ArtistTypeDto"/>.
    /// </returns>
    public static Result<ArtistTypeDto> FromArtistType(ArtistType artistType)
    {
        if (artistType is null)
        {
            return Result<ArtistTypeDto>.Failure(
                "Cannot convert ArtistType to ArtistTypeDto, value is null"
            );
        }

        return new ArtistTypeDto(
            artistType.Id,
            artistType.Name,
            artistType.Slug
        ).ToValueResult();
    }

    /// <summary>
    ///     Maps a collection of <see cref="ArtistType"/> to a collection of <see cref="ArtistTypeDto"/>.
    /// </summary>
    /// 
    /// <param name="artistTypes">
    ///     The collection of <see cref="ArtistType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ArtistTypeDto"/>.
    /// </returns>
    public static Result<IEnumerable<ArtistTypeDto>> FromArtistTypes(IEnumerable<ArtistType> artistTypes)
    {
        List<ArtistTypeDto> dtos = [];

        foreach (var artistType in artistTypes)
        {
            var result = FromArtistType(artistType);
            if (result.IsFailure)
            {
                return Result<IEnumerable<ArtistTypeDto>>.Failure(
                    $"Cannot convert artist types collection into artist type DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }

}