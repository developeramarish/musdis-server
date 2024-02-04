using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     The service for managing <see cref="Tag"/>s data.
/// </summary>
public interface ITagService
{
    /// <summary>
    ///     Creates <see cref="Tag"/> entity in database.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request to create an <see cref="Tag"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="Tag"/> value.
    /// </returns>
    Task<Result<Tag>> CreateAsync(
        CreateTagRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Updates <see cref="Tag"/> entity in the database.
    /// </summary>
    /// 
    /// <param name="id">
    ///     The identifier of the <see cref="Tag"/>.
    /// </param>
    /// <param name="request">
    ///     Request to update <see cref="Tag"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the update.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="Tag"/> value.
    /// </returns>
    Task<Result<Tag>> UpdateAsync(
        Guid id,
        UpdateTagRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes the <see cref="Tag"/> from the database.
    /// </summary>
    /// 
    /// <param name="tagId">
    ///     The identifier of the <see cref="Tag"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the deletion.
    /// </param>
    
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the deletion.
    /// </returns>
    Task<Result> DeleteAsync(Guid tagId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.Tags"/> queryable.
    /// </summary>
    /// <remarks>
    ///     Retrieved queryable should be used for reading purposes only. 
    /// </remarks>
    /// 
    /// <returns>
    ///     A no tracking <see cref="Tag"/>s queryable.
    /// </returns>
    IQueryable<Tag> GetQueryable();
}
