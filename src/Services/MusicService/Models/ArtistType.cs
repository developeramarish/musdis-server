namespace Musdis.MusicService.Models;

/// <summary>
/// Represents type of the <see cref="Artist"/> (e.g. band, musician, etc.).
/// </summary>
public class ArtistType
{
    /// <summary>
    /// The unique identifier of the <see cref="ArtistType"/>
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the <see cref="ArtistType"/>
    /// </summary>
    public required string Name { get; set; }

        /// <summary>
    /// The human-readable, unique identifier of the <see cref="ArtistType"/>.
    /// </summary>
    public required string Slug { get; set; }
}