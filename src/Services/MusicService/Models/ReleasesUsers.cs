namespace MusicService.Models;

/// <summary>
/// Represents many-to-many relationship between artists and releases. 
/// </summary>
public class ReleasesArtists
{
    /// <summary>
    /// Foreign key for release table.
    /// </summary>
    public required int ReleaseId { get; set; }

    /// <summary>
    /// Identifier of the user.
    /// </summary>
    public required int ArtistId { get; set; }
}