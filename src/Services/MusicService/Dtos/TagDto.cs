using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="Tag"/>.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the <see cref="Tag"/>.
/// </param>
/// <param name="Name">
///     The name of the <see cref="Tag"/>.
/// </param>
/// <param name="Slug">
///     The slug of the <see cref="Tag"/>.
/// </param>
public sealed record TagDto(
    Guid Id,
    string Name,
    string Slug
)
{
    /// <summary>
    ///     Maps a <see cref="Tag"/> to a <see cref="TagDto"/>.
    /// </summary>
    /// 
    /// <param name="tag">
    ///     The <see cref="Tag"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result"/> containing the mapped <see cref="TagDto"/>.
    /// </returns>
    public static Result<TagDto> FromTag(Tag tag)
    {
        if (tag is null)
        {
            return Result<TagDto>.Failure(
                "Cannot convert Tag to TagDto, value is null"
            );
        }

        return new TagDto(
            tag.Id,
            tag.Name,
            tag.Slug
        ).ToValueResult();
    }

    /// <summary>
    ///     Maps a collection of <see cref="Tag"/> to a collection of <see cref="TagDto"/>.
    /// </summary>
    /// 
    /// <param name="tags">
    ///     The collection of <see cref="Tag"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="Result{T}"/> containing the mapped collection of <see cref="TagDto"/>.
    /// </returns>
    public static Result<IEnumerable<TagDto>> FromTags(IEnumerable<Tag> tags)
    {
        List<TagDto> dtos = [];

        foreach (var tag in tags)
        {
            var result = FromTag(tag);
            if (result.IsFailure)
            {
                return Result<IEnumerable<TagDto>>.Failure(
                    $"Cannot convert tag collection into tag DTOs: {result.Error.Description}"
                );
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }

}