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
    /// 
    /// <param name="releaseType">
    ///     The <see cref="ReleaseType"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped <see cref="ReleaseTypeDto"/>.
    /// </returns>
    public static Result<ReleaseTypeDto> FromReleaseType(ReleaseType releaseType)
    {
        if (releaseType is null)
        {
            return Result<ReleaseTypeDto>.Failure(
                "Cannot convert ReleaseType to ReleaseTypeDto, value is null"
            );
        }

        return new ReleaseTypeDto(
            releaseType.Id,
            releaseType.Name,
            releaseType.Slug
        ).ToValueResult();
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
    ///     A <see cref="Result"/> containing the mapped collection of <see cref="ReleaseTypeDto"/>.
    /// </returns>
    public static Result<IEnumerable<ReleaseTypeDto>> FromReleaseTypes(IEnumerable<ReleaseType> releaseTypes)
    {
        List<ReleaseTypeDto> dtos = [];

        foreach (var releaseType in releaseTypes)
        {
            var result = FromReleaseType(releaseType);
            if (result.IsFailure)
            {
                return Result<IEnumerable<ReleaseTypeDto>>.Failure(
                    $"Cannot convert release types collection into release type DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }

}