namespace Musdis.MusicService.Models;

/// <summary>
/// Represents type of the <see cref="Artist"/> (e.g. band, musician, etc.).
/// </summary>
public class ArtistType
{
    /// <summary>
    ///     The unique identifier of the <see cref="ArtistType"/>
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the <see cref="ArtistType"/>
    /// </summary>
    /// <remarks>
    ///     It is unique for each <see cref="ArtistType"/>.
    /// </remarks>
    public required string Name { get; set; }

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="ArtistType"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     A collection of <see cref="Artist"/>s with this <see cref="ArtistType"/>.
    /// </summary>
    public ICollection<Artist>? Artists { get; set; }
}