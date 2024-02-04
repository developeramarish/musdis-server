namespace Musdis.MusicService.Models;

/// <summary>
/// Represents music release.
/// </summary>
public class Release
{
    /// <summary>
    ///     The unique identifier of the release.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The foreign key to <see cref="Models.ReleaseType"/> table.
    /// </summary>
    public required Guid ReleaseTypeId { get; set; }

    /// <summary>
    ///     The type of the <see cref="Release"/>.
    /// </summary>
    public ReleaseType? ReleaseType { get; set; }

    /// <summary>
    ///     The name of the <see cref="Release"/>.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="Release"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     The release date of the <see cref="Release"/>.
    /// </summary>
    public required DateTime ReleaseDate { get; set; }

    /// <summary>
    ///     A URL to the cover of the <see cref="Release"/>.
    /// </summary>
    public required string CoverUrl { get; set; }

    /// <summary>
    ///     A collection of <see cref="Artist"/>s participated in creation of this <see cref="Track"/>.
    /// </summary>
    public IEnumerable<Artist>? Artists { get; set; }

    /// <summary>
    ///     A collection of <see cref="Track"/>s of this <see cref="Release"/>.
    /// </summary>
    public IEnumerable<Track>? Tracks { get; set; }
}