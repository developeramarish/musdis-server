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
    /// 
    /// <param name="artists">
    ///     The collection of <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ArtistDto"/>.
    /// </returns>
    public Result<ArtistUserDto> FromArtistUser(ArtistUser artistUser)
    {
        if (artistUser is null)
        {
            return Result<ArtistUserDto>.Failure("Cannot convert null to ArtistUserDto.");
        }

        return new ArtistUserDto(
            artistUser.ArtistId,
            artistUser.UserId,
            artistUser.UserName
        ).ToValueResult();
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
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ArtistUserDto"/>.
    /// </returns>
    public Result<IEnumerable<ArtistUserDto>> FromArtistUsers(IEnumerable<ArtistUser> artistUsers)
    {
        if (artistUsers is null)
        {
            return Result<IEnumerable<ArtistUserDto>>.Failure(
                "Cannot convert null collection to ArtistUserDto collection."
            );
        }

        List<ArtistUserDto> dtos = [];

        foreach (var artistUser in artistUsers)
        {
            var result = FromArtistUser(artistUser);
            if (result.IsFailure)
            {
                return Result<IEnumerable<ArtistUserDto>>.Failure(
                    $"Cannot convert artist users collection to ArtistUserDto collection: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }
}