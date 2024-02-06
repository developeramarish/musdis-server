using Musdis.OperationResults;

namespace Musdis.MusicService.Models;

/// <summary>
/// Represents the type of release (e.g. album, single, EP, etc.).
/// </summary>
public class ReleaseType
{
    /// <summary>
    ///     The unique identifier of the <see cref="ReleaseType"/>
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the <see cref="ReleaseType"/>
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="ReleaseType"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     A collection of <see cref="Release"/>s with this <see cref="ReleaseType"/>.
    /// </summary>
    public IEnumerable<Release>? Releases { get; set; }
}