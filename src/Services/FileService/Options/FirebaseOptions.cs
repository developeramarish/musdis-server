namespace Musdis.FileService.Options;

/// <summary>
/// Represents options for Google Firebase.
/// </summary>
public class FirebaseOptions
{
    /// <summary>
    ///     The configuration path of the Firebase option.
    /// </summary>
    public const string Firebase = nameof(Firebase);

    /// <summary>
    ///     The project id of the Firebase project.
    /// </summary>
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    ///     The key environment variable name of the Firebase project.
    /// </summary>
    public string KeyEnvironmentVariableName { get; set; } = string.Empty;

    /// <summary>
    ///     The path to the Firebase project credentials.
    /// </summary>
    public string KeyPath { get; set; } = string.Empty;

    /// <summary>
    ///     The default bucket name of the Firebase project.
    /// </summary>
    public string DefaultBucketName { get; set; } = string.Empty;
}