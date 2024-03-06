namespace Musdis.FileService.Models;

/// <summary>
///     Represents a file.
/// </summary>
public sealed class FileDetails
{
    /// <summary>
    ///     The identifier of the file.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the file.
    /// </summary>
    public required string Name { get; set; }
        
    /// <summary>
    ///     The identifier of the user that owns the file.
    /// </summary>
    public required string OwnerId { get; set; }

    /// <summary>
    ///     The foreign key to the type of the file.
    /// </summary>
    public required Guid FileTypeId { get; set; }

    /// <summary>
    ///     The type of the file.
    /// </summary>
    public FileType? FileType { get; set; }
}