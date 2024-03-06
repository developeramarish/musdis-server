namespace Musdis.FileService.Models;

/// <summary>
///     The type of the file.
/// </summary>
public sealed class FileType
{
    /// <summary>
    ///     The identifier of the file type.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the file type.
    /// </summary>
    public required string Name { get; set; }
}