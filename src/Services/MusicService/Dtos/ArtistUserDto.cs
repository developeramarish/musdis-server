using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="ArtistUser"/>.
/// </summary>
/// 
/// <param name="ArtistId">
///     The identifier of the related <see cref="Artist"/>.
/// </param>
/// <param name="UserId">
///     The identifier of the related user.
/// </param>
/// <param name="UserName">
///     The name of the related user.
/// </param>
public sealed record ArtistUserDto(
    Guid ArtistId,
    string UserId,
    string UserName
)
{
    /// <summary>
    ///     Maps a collection of <see cref="Artist"/> to a collection of <see cref="ArtistDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure <paramref name="artistUser"/> is not null.
    /// </remarks>
    /// 
    /// <param name="artists">
    ///     The collection of <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped collection of <see cref="ArtistDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public ArtistUserDto FromArtistUser(ArtistUser artistUser)
    {
        if (artistUser is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert artist user into DTO, make sure it is not null"
            );
        }

        return new(
            artistUser.ArtistId,
            artistUser.UserId,
            artistUser.UserName
        );
    }

    /// <summary>
    ///     Maps a collection of <see cref="ArtistUser"/> to a collection of <see cref="ArtistUserDto"/>.
    /// </summary>
    /// 
    /// <param name="artistUsers">
    ///     The collection of <see cref="ArtistUser"/> to map.
    ///  </param>
    ///  
    /// <returns>
    ///     The mapped collection of <see cref="ArtistUserDto"/>.
    /// </returns>
    public IEnumerable<ArtistUserDto> FromArtistUsers(IEnumerable<ArtistUser> artistUsers)
    {
        return  artistUsers.Select(au => FromArtistUser(au));
    }
}