using Musdis.FileService.Exceptions;
using Musdis.FileService.Models;

namespace Musdis.FileService.Dtos;

public sealed record FileMetadataDto(
    Guid Id,
    string Path,
    string Url,
    string FileType
)
{
    /// <summary>
    ///     Converts a file metadata to a file metadata DTO.
    /// </summary>
    /// <remarks>
    ///     Make sure <see cref="FileMetadata.FileType"/> is not null.
    /// </remarks>
    /// 
    /// <param name="fileMetadata">
    ///     The file metadata to convert.
    /// </param>
    /// 
    /// <returns>
    ///     The converted file metadata DTO.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown when <see cref="FileMetadata.FileType"/> is null.
    /// </exception>
    public static FileMetadataDto FromFileMetadata(FileMetadata fileMetadata)
    {
        return new(
            fileMetadata.Id,
            fileMetadata.Path,
            fileMetadata.Url,
            fileMetadata.FileType
        );
    }

    /// <summary>
    ///     Converts a collection of file metadata to a collection of file metadata DTOs.
    /// </summary>
    /// <remarks>
    ///     Make sure every <see cref="FileMetadata.FileType"/> of <paramref name="filesMetadata"/> is not null.
    /// </remarks>    
    /// 
    /// <param name="filesMetadata">
    ///     The collection of files metadata to convert.
    /// </param>
    /// 
    /// <returns>
    ///     The converted collection of file metadata DTOs.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown when <see cref="FileMetadata.FileType"/> is null.
    /// </exception>
    public static IEnumerable<FileMetadataDto> FromFilesMetadata(IEnumerable<FileMetadata> filesMetadata)
    {
        return filesMetadata.Select(FromFileMetadata);
    }
}