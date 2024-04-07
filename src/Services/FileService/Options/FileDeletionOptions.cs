namespace Musdis.FileService.Options;

/// <summary>
///     Represents settings for file deletion.
/// </summary>
public sealed class FileDeletionOptions
{
    /// <summary>
    ///     The name of the settings section.
    /// </summary>
    public static string FileDeletionSettings { get; } = nameof(FileDeletionSettings);
    /// <summary>
    ///     The expiration time in hours.
    /// </summary>
    public required int ExpirationTimeInHours { get; set; }
}