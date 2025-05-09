namespace Musdis.MusicService.Models;

/// <summary>
///     Represents music track.
/// </summary>
public class Track
{
    /// <summary>
    ///     The unique identifier of the <see cref="Track"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The title of the track.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     The human-readable, unique identifier of the <see cref="Track"/>.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    ///     The identifier of the user from Identity service, who created this <see cref="Artist"/>.
    /// </summary>
    public required string CreatorId { get; set; }

    /// <summary>
    ///     The URL of the track.
    /// </summary>
    public required string AudioUrl { get; set; }

    /// <summary>
    ///     The identifier of the file from file service database.
    /// </summary>
    public required Guid AudioFileId { get; set; }

    /// <summary>
    ///     The URL of the cover image.
    /// </summary>
    public required string CoverUrl { get; set; }

    /// <summary>
    ///     The foreign key to <see cref="Models.Release"/> table.
    /// </summary>
    public required Guid ReleaseId { get; set; }

    /// <summary>
    ///     The release the track belongs to.
    /// </summary>
    public Release? Release { get; set; }

    /// <summary>
    ///     A collection of artists (author(s)) of this track.
    /// </summary>
    public ICollection<Artist>? Artists { get; set; }

    /// <summary>
    ///     A collection of <see cref="Tag"/>s associated with this <see cref="Track"/>.
    /// </summary>
    public ICollection<Tag>? Tags { get; set; }
}