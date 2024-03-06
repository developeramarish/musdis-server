namespace Musdis.FileService.Models;

/// <summary>
///     Represents a relation between a user's avatar and a file.
/// </summary>
public sealed class AvatarFile
{
    /// <summary>
    ///     The identifier of the user that owns the avatar.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    ///     The identifier of the file.
    /// </summary>
    public required Guid FileDetailsId { get; set; }

    /// <summary>
    ///     An associated file.
    /// </summary>
    public FileDetails? FileDetails { get; set; }
}