using Musdis.FileService.Requests;
using Musdis.OperationResults;

namespace Musdis.FileService.Services.Storage;

/// <summary>
///     A service for manage track related files.
/// </summary>
public interface ITrackStorageService
{
    /// <summary>
    ///     Uploads a track file.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request to upload the track file.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the uploaded file.
    /// </returns>
    Task<Result<Uri>> UploadTrackFileAsync(
        UploadTrackRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Uploads a list of track files.
    /// </summary>
    /// 
    /// <param name="requests">
    ///     The collection of requests to upload the track files.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the list of URLs of the uploaded files.
    /// </returns>
    Task<Result<List<Uri>>> UploadTrackFilesUrlAsync(
        IEnumerable<UploadTrackRequest> requests,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URL of the track file.
    /// </summary>
    /// 
    /// <param name="trackId">
    ///     The identifier of the track.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the track file.
    /// </returns>
    Task<Result<Uri>> GetTrackFileUrlAsync(
        Guid trackId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URLs of the track files.
    /// </summary>
    /// 
    /// <param name="trackIds">
    ///     The identifiers of the tracks.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the list of URLs of the track files.
    /// </returns>
    Task<Result<List<Uri>>> GetTrackFileUrlsAsync(
        IList<Guid> trackIds,
        CancellationToken cancellationToken = default
    );
}