namespace Musdis.MusicService.Models;

/// <summary>
/// Represents many-to-many relationship between <see cref="Models.Tag"/>s and <see cref="Models.Track"/>s. 
/// </summary>
public class TagTrack
{
    /// <summary>
    /// The foreign key to <see cref="Models.Track"/>s table.
    /// </summary>
    public required Guid TrackId { get; set; }

    /// <summary>
    /// The <see cref="Models.Track"/> associated with this object.
    /// </summary>
    public Track? Track { get; set; }

    /// <summary>
    /// The foreign key to <see cref="Models.Tag"/>s table.
    /// </summary>
    public required Guid TagId { get; set; }

    /// <summary>
    /// The <see cref="Models.Tag"/> associated with this object.
    /// </summary>
    public Tag? Tag { get; set; }
}