namespace FileService.Options;

/// <summary>
/// Represents options for Google Firebase.
/// </summary>
public class FirebaseOptions
{
    public const string Firebase = nameof(Firebase);

    public string ProjectId { get; set; } = string.Empty;
    public string KeyEnvironmentVariableName { get; set; } = string.Empty;
    public string KeyPath { get; set; } = string.Empty;
    public string DefaultBucketName { get; set; } = string.Empty;
}