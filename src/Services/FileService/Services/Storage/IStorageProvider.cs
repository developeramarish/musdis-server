using Musdis.OperationResults;

namespace Musdis.FileService.Services.Storage;

/// <summary>
///     A file storage provider.
/// </summary>
public interface IStorageProvider
{
    /// <summary>
    ///     Uploads the file to storage.
    /// </summary>
    /// 
    /// <param name="fileName">
    ///     The name of the file.
    /// </param>
    /// <param name="file">
    ///     The file to be uploaded.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the uploaded file.
    /// </returns>
    Task<Result<Uri>> UploadFileAsync(
        string fileName,
        IFormFile file,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes the file from the storage.
    /// </summary>
    /// 
    /// <param name="fileName">
    ///     The name of the file.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the result of the operation.
    /// </returns>
    Task<Result> DeleteFileAsync(
        string fileName, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URL of the file.
    /// </summary>
    /// 
    /// <param name="fileName">
    ///     The name of the file.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the file.
    /// </returns>
    Task<Result<Uri>> GetFileUrlAsync(
        string fileName, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URLs of the files.
    /// </summary>
    /// 
    /// <param name="fileNames">
    ///     The names of the files.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URLs of the files.
    /// </returns>
    Task<Result<List<Uri>>> GetFileUrlsAsync(
        IList<string> fileNames,
        CancellationToken cancellationToken = default
    );
}