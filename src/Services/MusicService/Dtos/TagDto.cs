using Musdis.MusicService.Exceptions;
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
    /// <remarks>
    ///     Make sure <paramref name="tag"/> is not null.
    /// </remarks>
    /// 
    /// <param name="tag">
    ///     The <see cref="Tag"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped <see cref="TagDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public static TagDto FromTag(Tag tag)
    {
        if (tag is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert tag into DTO, make sure it is not null."
            );
        }

        return new(
            tag.Id,
            tag.Name,
            tag.Slug
        );
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
    ///     The mapped collection of <see cref="TagDto"/>.
    /// </returns>
    public static IEnumerable<TagDto> FromTags(IEnumerable<Tag> tags)
    {
        return tags.Select(t => FromTag(t));
    }

}