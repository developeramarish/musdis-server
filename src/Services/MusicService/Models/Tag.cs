namespace Musdis.MusicService.Models;

/// <summary>
///     Represents the tag for tracks.
/// </summary>
public class Tag
{
    /// <summary>
    ///     The unique identifier of the <see cref="Tag"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the <see cref="Tag"/>.
    /// </summary>
    /// <remarks> 
    ///     It is unique for each <see cref="Tag"/>.
    /// </remarks>
    public required string Name { get; set; } 

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="Artist"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     A collection of <see cref="Track"/>s associated with this <see cref="Tag"/>.
    /// </summary>
    public ICollection<Track>? Tracks { get; set; }
}