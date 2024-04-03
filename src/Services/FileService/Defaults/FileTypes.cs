using System.Collections.Immutable;

namespace Musdis.FileService.Defaults;

/// <summary>
///     The default file types.
/// </summary>
public static class FileTypes
{
    /// <summary>
    ///     The supported file types.
    /// </summary>
    public static ImmutableArray<string> SupportedTypes { get; } = [Audio, Image];

    /// <summary>
    ///     The audio file type.
    /// </summary>
    public static string Audio => "audio";

    /// <summary>
    ///     The image file type.
    /// </summary>
    public static string Image => "image";
}