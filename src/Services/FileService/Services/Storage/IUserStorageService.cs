using Musdis.FileService.Requests;
using Musdis.OperationResults;

namespace Musdis.FileService.Services.Storage;

/// <summary>
///     A service for manage user related files.
/// </summary>
public interface IUserStorageService
{
    /// <summary>
    ///     Uploads a user avatar.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request to upload the user avatar.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the URL of the uploaded file.
    /// </returns>
    Task<Result<Uri>> UploadUserAvatarAsync(
        UploadUserAvatarRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets the URL of the user avatar.
    /// </summary>
    /// 
    /// <param name="userId">
    ///     The identifier of the user.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    Task<Result<Uri>> GetUserAvatarUrlAsync(
        string userId,
        CancellationToken cancellationToken = default
    );
}