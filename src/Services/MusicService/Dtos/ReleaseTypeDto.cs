using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="ReleaseType"/>.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the <see cref="ReleaseType"/>.
/// </param>
/// <param name="Name">
///     The name of the <see cref="ReleaseType"/>.
/// </param>
/// <param name="Slug">
///     The slug of the <see cref="ReleaseType"/>.
/// </param>
public sealed record ReleaseTypeDto(
    Guid Id,
    string Name,
    string Slug
)
{
    /// <summary>
    ///     Maps a <see cref="ReleaseType"/> to an <see cref="ReleaseTypeDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure <paramref name="releaseType"/> is not null.
    /// </remarks>
    /// 
    /// <param name="releaseType">
    ///     The <see cref="ReleaseType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped <see cref="ReleaseTypeDto"/>.
    /// </returns>
    public static ReleaseTypeDto FromReleaseType(ReleaseType releaseType)
    {
        if (releaseType is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert artist type into DTO, make sure it is not null"
            );
        }

        return new ReleaseTypeDto(
            releaseType.Id,
            releaseType.Name,
            releaseType.Slug
        );
    }

    /// <summary>
    ///     Maps a collection of <see cref="ReleaseType"/> to a collection of <see cref="ReleaseTypeDto"/>.
    /// </summary>
    /// 
    /// <param name="releaseTypes">
    ///     The collection of <see cref="ReleaseType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped collection of <see cref="ReleaseTypeDto"/>.
    /// </returns>
    public static IEnumerable<ReleaseTypeDto> FromReleaseTypes(IEnumerable<ReleaseType> releaseTypes)
    {
        return releaseTypes.Select(rt => FromReleaseType(rt));
    }
}