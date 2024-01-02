namespace MusicService.Models;

/// <summary>
/// Represents music release.
/// </summary>
public class Release
{
    /// <summary>
    /// Unique identifier of the release.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to Release type table.
    /// </summary>
    public required int ReleaseTypeId { get; set; }

    /// <summary>
    /// Type of the release (e.g. Album or Single).
    /// </summary>
    public ReleaseType? Type { get; set; }

    /// <summary>
    /// Name of the release.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Release date of the release.
    /// </summary>
    public required DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Cover url of the release.
    /// </summary>
    public required string CoverUrl { get; set; }
}