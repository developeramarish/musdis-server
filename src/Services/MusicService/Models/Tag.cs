namespace Musdis.MusicService.Models;

/// <summary>
/// Represents tag for tracks.
/// </summary>
public class Tag
{
    /// <summary>
    /// The unique identifier of the <see cref="Tag"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the <see cref="Tag"/>.
    /// </summary>
    public required string Name { get; set; } 

    /// <summary>
    /// A collection of <see cref="Track"/>s associated with this <see cref="Tag"/>.
    /// </summary>
    public IEnumerable<Track>? Tracks { get; set; }
}