namespace Musdis.FileService.Models;

/// <summary>
///     Represents a file.
/// </summary>
public sealed class FileMetadata
{
    /// <summary>
    ///     The identifier of the file.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The URL of the file.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    ///     The name of the file.
    /// </summary>
    public required string Path { get; set; }

    /// <summary>
    ///     The type of the file.
    /// </summary>
    public required string FileType { get; set; }

    /// <summary>
    ///     Indicates if the file is used.
    /// </summary>
    public required bool IsUsed { get; set; }
}