using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     A service for managing <see cref="ArtistType"/>s data.
/// </summary>
public interface IArtistTypeService
{
    /// <summary>
    ///     Creates a new <see cref="ArtistType"/> object in the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="request">
    ///     A request to create an <see cref="ArtistType"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="ArtistType"/> object.
    /// </returns>
    Task<Result<ArtistType>> CreateAsync(
        CreateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Updates an <see cref="ArtistType"/> entity in the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="id">
    ///     The identifier of the <see cref="ArtistType"/>.
    /// </param>
    /// <param name="request">
    ///     A request to update the <see cref="ArtistType"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the update.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="ArtistType"/> value.
    /// </returns>
    Task<Result<ArtistType>> UpdateAsync(
        Guid id,
        UpdateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    ///     Deletes the <see cref="ArtistType"/> from the database.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="SaveChangesAsync(CancellationToken)"/> to save changes to the database.
    /// </remarks>
    /// 
    /// <param name="artistTypeId">
    ///     The identifier of the <see cref="ArtistType"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the deletion.
    /// </param>
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the deletion.
    /// </returns>
    Task<Result> DeleteAsync(
        Guid artistTypeId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.ArtistTypes"/> queryable.
    /// </summary>
    /// <remarks>
    ///     Retrieved queryable should be used for reading purposes only. 
    /// </remarks>
    /// 
    /// <returns>
    ///     A no tracking <see cref="ArtistType"/>s queryable.
    /// </returns>
    IQueryable<ArtistType> GetQueryable();

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