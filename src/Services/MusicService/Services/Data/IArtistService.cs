using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     <see cref="Musdis.MusicService.Data.MusicServiceDbContext"/> wrapper.
/// </summary>
public interface IArtistService
{
    /// <summary>
    ///     Creates <see cref="Artist"/> entity in database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync"/> to save changes to database.
    /// </remarks>
    /// 
    /// <param name="request">
    ///     Request to create an <see cref="Artist"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel operation.
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
    ///     Updates <see cref="Artist"/> entity in database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync"/> to save changes to database.
    /// </remarks>
    /// 
    /// <param name="request">
    ///     Request to update <see cref="Artist"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="Artist"/> value.
    /// </returns>
    Task<Result<Artist>> UpdateAsync(
        UpdateArtistRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes <see cref="Artist"/> from database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync"/> to save changes to database.
    /// </remarks>
    /// 
    /// <param name="artistId">
    ///     Identifier of the <see cref="Artist"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel operation.
    /// </param>
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of an operation.
    /// </returns>
    Task<Result> DeleteAsync(Guid artistId, CancellationToken cancellationToken = default);

    /// <summary>
    ///      changes to database.
    /// </summary>
    /// 
    /// <param name="cancellationToken">
    ///     Token to cancel operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of an operation.
    /// </returns>
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.Artists"/> queryable.
    /// </summary>
    /// 
    /// <returns>
    ///     <see cref="Artist"/>s queryable.
    /// </returns>
    IQueryable<Artist> GetQueryable();
}