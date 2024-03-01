using System.Data;

using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Dtos;

/// <summary>
///     Represents a DTO for <see cref="Artist"/>.
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
/// <param name="Users">
///     A collection of users that are participants of the <see cref="Artist"/>.
/// </param>
public sealed record ArtistDto(
    Guid Id,
    string Name,
    string Slug,
    string CoverUrl,
    string CreatorId,
    ArtistTypeDto Type,
    IEnumerable<ArtistUserDto> Users
)
{
    /// <summary>
    ///     Maps an <see cref="Artist"/> to an <see cref="ArtistDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure <see cref="Artist.ArtistType"/> and <see cref="Artist.ArtistUsers"/> are not null.
    /// </remarks>
    /// 
    /// <param name="artist">
    ///     The <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///    The mapped <see cref="ArtistDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown if method called incorrectly, see remarks.
    /// </exception>
    public static ArtistDto FromArtist(Artist artist)
    {
        if (artist?.ArtistType is null || artist.ArtistUsers is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert Artist to ArtistDto, check if ArtistType and ArtistUsers properties are not null."
            );
        }

        return new(
            artist.Id,
            artist.Name,
            artist.Slug,
            artist.CoverUrl,
            artist.CreatorId,
            ArtistTypeDto.FromArtistType(artist.ArtistType),
            ArtistUserDto.FromArtistUsers(artist.ArtistUsers)
        );
    }

    /// <summary>
    ///     Maps a collection of <see cref="Artist"/> to a collection of <see cref="ArtistDto"/>.
    /// </summary>
    /// <remarks>
    ///     Make sure every artists's <see cref="Artist.ArtistType"/> and <see cref="Artist.ArtistUsers"/> 
    ///     in collection are not null.
    /// </remarks>
    /// 
    /// <param name="artists">
    ///     The collection of <see cref="Artist"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped collection of <see cref="ArtistDto"/>.
    /// </returns>
    public static IEnumerable<ArtistDto> FromArtists(IEnumerable<Artist> artists)
    {
        return artists.Select(a => FromArtist(a));
    }
}