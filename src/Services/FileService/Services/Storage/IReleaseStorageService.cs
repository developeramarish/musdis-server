using Musdis.FileService.Requests;
using Musdis.OperationResults;

namespace Musdis.FileService.Services.Storage;

/// <summary>
///     A service for manage release related files.
/// </summary>
public interface IReleaseStorageService
{
    /// <summary>
    ///     Uploads release cover file.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request to upload the release cover file.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the uploaded file.
    /// </returns>
    Task<Result<Uri>> UploadReleaseCoverFileAsync(
        UploadReleaseCoverRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URL of the release cover file.
    /// </summary>
    /// 
    /// <param name="releaseId">
    ///     The identifier of the release.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the release cover file.
    /// </returns>
    Task<Result<Uri>> GetReleaseCoverFileUrlAsync(
        Guid releaseId,
        CancellationToken cancellationToken = default
    );
}