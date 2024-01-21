namespace Musdis.MusicService.Models;

/// <summary>
/// Represents musicians, bands or other type of songwriters.
/// </summary>
public class Artist
{
    /// <summary>
    /// The unique identifier of the <see cref="Artist"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the <see cref="Artist"/>.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The foreign key to <see cref="Models.ArtistType"/> table.
    /// </summary>
    public required int ArtistTypeId { get; set; }

    /// <summary>
    /// The type of the <see cref="Artist"/> (e.g. band or musician).
    /// </summary>
    public ArtistType? ArtistType { get; set; }

    /// <summary>
    /// Collection of <see cref="Track"/>s of this <see cref="Artist"/>.
    /// </summary>
    public IEnumerable<Track>? Tracks { get; set; }
}
