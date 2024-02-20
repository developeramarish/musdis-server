using Musdis.MusicService.Exceptions;
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
    /// <remarks>
    ///     Make sure <paramref name="artistType"/> is not null.
    /// </remarks>
    /// 
    /// <param name="artistType">
    ///     The <see cref="ArtistType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped <see cref="ArtistTypeDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public static ArtistTypeDto FromArtistType(ArtistType artistType)
    {
        if (artistType is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert artist type into DTO, make sure it is not null."
            );
        }

        return new(
            artistType.Id,
            artistType.Name,
            artistType.Slug
        );
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
    ///     The mapped collection of <see cref="ArtistTypeDto"/>.
    /// </returns>
    public static IEnumerable<ArtistTypeDto> FromArtistTypes(IEnumerable<ArtistType> artistTypes)
    {
        return artistTypes.Select(a => FromArtistType(a));
    }

}