namespace MusicService.Models;

/// <summary>
/// Represents music genre.
/// </summary>
public class Genre
{
    /// <summary>
    /// Unique identifier of the genre.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the genre.
    /// </summary>
    public required string Name { get; set; }
}