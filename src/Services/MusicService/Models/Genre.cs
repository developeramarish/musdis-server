namespace MusicService.Models;

/// <summary>
/// Represents music genre.
/// </summary>
public class Genre
{
    /// <summary>
    /// The unique identifier of the <see cref="Genre"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the <see cref="Genre"/>.
    /// </summary>
    public required string Name { get; set; } 
}