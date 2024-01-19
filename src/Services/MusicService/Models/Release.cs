namespace Musdis.MusicService.Models;

/// <summary>
/// Represents music release.
/// </summary>
public class Release
{
    /// <summary>
    /// The unique identifier of the release.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The foreign key to <see cref="ReleaseType"/> table.
    /// </summary>
    public required Guid ReleaseTypeId { get; set; }

    /// <summary>
    /// The type of the <see cref="Release"/>.
    /// </summary>
    public ReleaseType? Type { get; set; }

    /// <summary>
    /// The name of the <see cref="Release"/>.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The release date of the <see cref="Release"/>.
    /// </summary>
    public required DateTime ReleaseDate { get; set; }

    /// <summary>
    /// URL to the cover of the <see cref="Release"/>.
    /// </summary>
    public required string CoverUrl { get; set; }
}