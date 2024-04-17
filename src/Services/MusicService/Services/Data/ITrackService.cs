using Musdis.MusicService.Dtos;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;

namespace Musdis.MusicService.Services.Data;

/// <summary>
///     A service for managing <see cref="Track"/>s data.
/// </summary>
public interface ITrackService
{
    /// <summary>
    ///     Creates <see cref="Track"/> entity in database.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request to create an <see cref="Track"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="Track"/> value.
    /// </returns>
    Task<Result<TrackDto>> CreateAsync(
        CreateTrackRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Creates <see cref="Track"/> entity in database with specified <see cref="Track.ReleaseId"/>.
    /// </summary>
    /// 
    /// <param name="trackInfo">
    ///     The <see cref="CreateReleaseRequest.TrackInfo"/> to generate <see cref="Track"/> from.
    /// </param>
    /// <param name="release">
    ///     The <see cref="Release"/> of the <see cref="Track"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the creation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains
    ///     <see cref="Result{TValue}"/> of an operation with created <see cref="Track"/> value.
    /// </returns>
    Task<Result<TrackDto>> CreateForReleaseAsync(
        CreateReleaseRequest.TrackInfo trackInfo,
        Release release,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Updates <see cref="Track"/> entity in the database.
    /// </summary>
    /// 
    /// <param name="id">
    ///     The identifier of the <see cref="Track"/>.
    /// </param>
    /// <param name="request">
    ///     Request to update <see cref="Track"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the update.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation. The task result contains 
    ///     <see cref="Result{TValue}"/> of an operation with updated <see cref="Track"/> value.
    /// </returns>
    Task<Result<TrackDto>> UpdateAsync(
        Guid id,
        UpdateTrackRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Deletes the <see cref="Track"/> from the database.
    /// </summary>
    /// 
    /// <param name="trackId">
    ///     The identifier of the <see cref="Track"/>
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the deletion.
    /// </param>
    
    /// <returns>
    ///     A task representing asynchronous operation. 
    ///     The task result contains <see cref="Result"/> of the deletion.
    /// </returns>
    Task<Result> DeleteAsync(Guid trackId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets read access to <see cref="MusicService.Data.MusicServiceDbContext.Tracks"/> queryable.
    /// </summary>
    /// <remarks>
    ///     Retrieved queryable should be used for reading purposes only. 
    /// </remarks>
    /// 
    /// <returns>
    ///     A no tracking <see cref="Track"/>s queryable.
    /// </returns>
    IQueryable<Track> GetQueryable();

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
