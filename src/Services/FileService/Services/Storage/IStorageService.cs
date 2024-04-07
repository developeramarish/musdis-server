using Musdis.FileService.Dtos;
using Musdis.FileService.Models;
using Musdis.OperationResults;

namespace Musdis.FileService.Services.Storage;

using FilesMetadataResult = Result<IList<FileMetadataDto>>;

/// <summary>
///     A service for managing file storage.
/// </summary>
public interface IStorageService
{
    /// <summary>
    ///     Uploads a file to the storage.
    /// </summary>
    /// 
    /// <param name="file">
    ///     The file to be uploaded.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the metadata of the uploaded file.
    /// </returns>
    Task<Result<FileMetadataDto>> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Uploads multiple files to the storage.
    /// </summary>
    /// 
    /// <param name="files">
    ///     The files to be uploaded.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the metadata of the uploaded files.
    /// </returns>
    Task<FilesMetadataResult> UploadFilesAsync(
        IList<IFormFile> files,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes a file from the storage.
    /// </summary>
    /// 
    /// <param name="id">
    ///     The identifier of the file to be deleted.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the result of the operation.
    /// </returns>
    Task<Result> DeleteFileAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes a file from the storage.
    /// </summary>
    /// 
    /// <param name="url">
    ///     The URL of the file to be deleted.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the result of the operation.
    /// </returns>
    Task<Result> DeleteFileAsync(
        string url,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the metadata of a file.
    /// </summary>
    /// 
    /// <param name="id">
    ///     The identifier of the file.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the metadata of the file.
    /// </returns>
    Task<Result<FileMetadataDto>> GetFileMetadataAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the metadata of multiple files.
    /// </summary>
    /// 
    /// <param name="ids">
    ///     The identifiers of the files.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     The task that represents the asynchronous operation.
    ///     The result contains the metadata of the files.
    /// </returns>
    Task<FilesMetadataResult> GetFilesMetadataAsync(
        IList<Guid> ids,
        CancellationToken cancellationToken = default
    );
}