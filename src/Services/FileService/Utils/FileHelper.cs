using System.Collections.Immutable;

using Musdis.FileService.Defaults;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.FileService.Utils;

/// <summary>
///     Helper methods for file extensions.
/// </summary>
public static class FileHelper
{
    /// <summary>
    ///     The map of file extensions and their types.
    /// </summary>
    public static ImmutableDictionary<string, string> ExtensionMap { get; } =
        new Dictionary<string, string>()
        {
            { ".mp3", FileTypes.Audio },
            { ".wav", FileTypes.Audio },
            { ".ogg", FileTypes.Audio },
            { ".jpeg", FileTypes.Image },
            { ".jpg", FileTypes.Image },
            { ".gif", FileTypes.Image },
            { ".png", FileTypes.Image },
            { ".webp", FileTypes.Image },
            { ".bmp", FileTypes.Image },
            { ".svg", FileTypes.Image },
        }.ToImmutableDictionary();

    /// <summary>
    ///     Gets the type of the file extension.
    /// </summary>
    /// 
    /// <param name="extension">
    ///     The file extension.
    /// </param>
    /// <returns>
    ///     The name of the type of the file. 
    /// </returns>
    public static Result<string> GetFileType(string extension)
    {
        if (ExtensionMap.TryGetValue(extension, out var type))
        {
            return type.ToValueResult();
        }

        return Result<string>.Failure("File extension is not supported.");
    }

    /// <summary>
    ///     Checks if the extension is supported.
    /// </summary>
    /// 
    /// <param name="extension">
    ///     The file extension.
    /// </param>
    /// <returns>
    ///     True if the extension is supported, false otherwise.
    /// </returns>
    public static bool IsExtensionSupported(string extension)
    {
        return GetFileType(extension).IsSuccess;
    }
}