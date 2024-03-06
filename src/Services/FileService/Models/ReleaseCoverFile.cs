namespace Musdis.FileService.Models;

/// <summary>
///     Represents a relation between a release's cover and a file.
/// </summary>
public sealed class ReleaseCoverFile
{
    /// <summary>
    ///     The identifier of the release.
    /// </summary>
    public required Guid ReleaseId { get; set; }

    /// <summary>
    ///     The identifier of the file.
    /// </summary>
    public required Guid FileDetailsId { get; set; }

    /// <summary>
    ///     An associated file.
    /// </summary>
    public FileDetails? FileDetails { get; set; }
}