namespace MusicService.Models;

/// <summary>
/// Represents music track.
/// </summary>
public class Track
{
    /// <summary>
    /// The unique identifier of the <see cref="Track"/>.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The title of the track.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// The slug of the track.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// The foreign key to <see cref="Models.Release"/> table.
    /// </summary>
    public required Guid ReleaseId { get; set; }

    /// <summary>
    /// The release the track belongs to.
    /// </summary>
    public Release? Release { get; set; }

    /// <summary>
    /// The foreign key to <see cref="Models.Artist"/> table.
    /// </summary>
    public required Guid ArtistId { get; set; }

    /// <summary>
    /// The artist (author(s)) of the track.
    /// </summary>
    public Artist? Artist { get; set; }

    /// <summary>
    /// A collection of <see cref="Tag"/>s associated with this <see cref="Track"/>.
    /// </summary>
    public IEnumerable<Tag>? Tags { get; set; }
}