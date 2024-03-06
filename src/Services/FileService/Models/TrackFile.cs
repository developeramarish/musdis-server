namespace Musdis.FileService.Models;

/// <summary>
///     Represents a relation between a track and a file.
/// </summary>
public sealed class TrackFile
{
    /// <summary>
    ///     The identifier of the track.
    /// </summary>
    public required Guid TrackId { get; set; }

    /// <summary>
    ///     The identifier of the file.
    /// </summary>
    public required Guid FileDetailsId { get; set; }

    /// <summary>
    ///     An associated file.
    /// </summary>
    public FileDetails? FileDetails { get; set; }
}