using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     The service for managing <see cref="Artist"/>s data.
/// </summary>
public interface IArtistService
{
    /// <summary>
    ///     Creates <see cref="Artist"/> entity in database.
    /// </summary>
    ///
    /// <param name="request">
    ///     The request to create an <see cref="Artist"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="Artist"/> value.
    /// </returns>
    Task<Result<Artist>> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Updates <see cref="Artist"/> entity in the database.
    /// </summary>
    /// 
    /// <param name="id">
    ///     The identifier of the <see cref="Artist"/>.
    /// </param>
    /// <param name="request">
    ///     Request to update <see cref="Artist"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the update.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="Artist"/> value.
    /// </returns>
    Task<Result<Artist>> UpdateAsync(
        Guid id,
        UpdateArtistRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes the <see cref="Artist"/> from the database.
    /// </summary>
    /// 
    /// <param name="artistId">
    ///     The identifier of the <see cref="Artist"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the deletion.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the deletion.
    /// </returns>
    Task<Result> DeleteAsync(Guid artistId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.Artists"/> queryable.
    /// </summary>
    /// <remarks>
    ///     Retrieved queryable should be used for reading purposes only. 
    /// </remarks>
    /// 
    /// <returns>
    ///     A no tracking <see cref="Artist"/>s queryable.
    /// </returns>
    IQueryable<Artist> GetQueryable();
}