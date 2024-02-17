namespace Musdis.MusicService.Models;

/// <summary>
/// Represents many-to-many relationship between <see cref="Models.Artist"/>s and <see cref="Models.Release"/>s. 
/// </summary>
public class ReleaseArtist
{
    /// <summary>
    ///     A foreign key for release table.
    /// </summary>
    public required Guid ReleaseId { get; set; }

    /// <summary>
    ///     The <see cref="Models.Release"/> associated with this object.
    /// </summary>
    public Release? Release { get; set; }

    /// <summary>
    ///     The identifier of the artist.
    /// </summary>
    public required Guid ArtistId { get; set; }

    /// <summary>
    ///     The <see cref="Models.Artist"/> associated with this object.
    /// </summary>
    public Artist? Artist { get; set; }
}