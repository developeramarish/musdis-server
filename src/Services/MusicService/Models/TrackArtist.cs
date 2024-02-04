namespace Musdis.MusicService.Models;

/// <summary>
///     Represents many-to-many relationship between <see cref="Models.Track"/>s and <see cref="Models.Artist"/>s. 
/// </summary>
public class TrackArtist
{
    /// <summary>
    ///     The foreign key to <see cref="Models.Track"/>s table.
    /// </summary>
    public required Guid TrackId { get; set; }

    /// <summary>
    ///     The <see cref="Models.Track"/> associated with this object.
    /// </summary>
    public Track? Track { get; set; }

    /// <summary>
    ///     The foreign key to <see cref="Models.Artist"/>s table.
    /// </summary>
    public required Guid ArtistId { get; set; }

    /// <summary>
    ///     The <see cref="Models.Artist"/> associated with this object.
    /// </summary>
    public Artist? Artist { get; set; }
}