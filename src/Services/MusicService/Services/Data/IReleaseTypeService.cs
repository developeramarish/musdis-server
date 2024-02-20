using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     A service for managing <see cref="ReleaseType"/>s data.
/// </summary>
public interface IReleaseTypeService
{
    /// <summary>
    ///     Creates a new <see cref="ReleaseType"/> object in the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="request">
    ///     A request to create an <see cref="ReleaseType"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="ReleaseType"/> object.
    /// </returns>
    Task<Result<ReleaseType>> CreateAsync(
        CreateReleaseTypeRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Updates an <see cref="ReleaseType"/> entity in the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="releaseTypeId">
    ///     The identifier of the <see cref="ReleaseType"/>.
    /// </param>
    /// <param name="request">
    ///     A request to update the <see cref="ReleaseType"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the update.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="ReleaseType"/> value.
    /// </returns>
    Task<Result<ReleaseType>> UpdateAsync(
        Guid releaseTypeId,
        UpdateReleaseTypeRequest request,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    ///     Deletes the <see cref="ReleaseType"/> from the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="releaseTypeId">
    ///     The identifier of the <see cref="ReleaseType"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the deletion.
    /// </param>
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the deletion.
    /// </returns>
    Task<Result> DeleteAsync(
        Guid releaseTypeId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.ReleaseTypes"/> queryable.
    /// </summary>
    /// <remarks>
    ///     Retrieved queryable should be used for reading purposes only. 
    /// </remarks>
    /// 
    /// <returns>
    ///     A no tracking <see cref="ReleaseType"/>s queryable.
    /// </returns>
    IQueryable<ReleaseType> GetQueryable();

    /// <summary>
    ///     Saves changes to the database.
    /// </summary>
    /// 
    /// <param name="cancellationToken">
    ///     A token to cancel operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the saving.
    /// </returns>
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
