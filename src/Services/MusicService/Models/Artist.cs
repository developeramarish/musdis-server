namespace Musdis.MusicService.Models;

/// <summary>
///     Represents musicians, bands or other type of songwriters.
/// </summary>
public class Artist
{
    // Database rows.

    /// <summary>
    ///     The unique identifier of the <see cref="Artist"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the <see cref="Artist"/>.
    /// </summary>
    /// <remarks>
    ///     It is unique for each <see cref="Artist"/>.
    /// </remarks>
    public required string Name { get; set; }

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="Artist"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     A URL to the cover of the <see cref="Artist"/>.
    /// </summary>
    public required string CoverUrl { get; set; }

    /// <summary>
    ///     The identifier of the file from file service database.
    /// </summary>
    public required Guid CoverFileId { get; set; }

    /// <summary>
    ///     The foreign key to <see cref="Models.ArtistType"/> table.
    /// </summary>
    public required Guid ArtistTypeId { get; set; }

    /// <summary>
    ///     The identifier of the user from Identity service, who created this <see cref="Artist"/>.
    /// </summary>
    public required string CreatorId { get; set; }

    // Related entities.

    /// <summary>
    ///     The type of the <see cref="Artist"/> (e.g. band or musician).
    /// </summary>
    public ArtistType? ArtistType { get; set; }

    /// <summary>
    ///     A collection of <see cref="Track"/>s of this <see cref="Artist"/>.
    /// </summary>
    public ICollection<Track>? Tracks { get; set; }

    /// <summary>
    ///     A collection of <see cref="ArtistUser"/>s of this <see cref="Artist"/>.
    /// </summary>
    public ICollection<ArtistUser>? ArtistUsers { get; set; }

    /// <summary>
    ///     A collection of <see cref="Release"/>s of this <see cref="Artist"/>.
    /// </summary>
    public ICollection<Release>? Releases { get; set; }
}
