using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Validation;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="ITrackService"/>
public sealed class TrackService : ITrackService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateTrackRequest> _createRequestValidator;
    private readonly IValidator<UpdateTrackRequest> _updateRequestValidator;

    public TrackService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateTrackRequest> createRequestValidator,
        IValidator<UpdateTrackRequest> updateRequestValidator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<Result<Track>> CreateAsync(
        CreateTrackRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create a Track, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<Track>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Track>(
            request.Title,
            cancellationToken
        );

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<Track>();
        }

        var track = new Track
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = slugResult.Value,
            ReleaseId = request.ReleaseId
        };

        await _dbContext.Tracks.AddAsync(track, cancellationToken);

        return track.ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid trackId, CancellationToken cancellationToken = default)
    {
        var track = await _dbContext.Tracks.FirstOrDefaultAsync(
            t => t.Id == trackId,
            cancellationToken
        );
        if (track is null)
        {
            return new NoContentError(
                $"Cannot delete, track with id = {trackId} not found"
            ).ToResult();
        }

        _dbContext.Tracks.Remove(track);

        return Result.Success();
    }

    public async Task<Result<Track>> UpdateAsync(
        Guid id,
        UpdateTrackRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create a Track, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<Track>();
        }

        var track = await _dbContext.Tracks
            .Include(t => t.Tags)
            .Include(t => t.Artists)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (track is null)
        {
            return new NotFoundError(
                $"Cannot update track with id = {id}, it is not found."
            ).ToValueResult<Track>();
        }

        if (request.Title is not null)
        {
            var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Track>(
                request.Title,
                cancellationToken
            );
            if (slugResult.IsFailure)
            {
                return slugResult.Error.ToValueResult<Track>();
            }

            track.Title = request.Title;
            track.Slug = slugResult.Value;

            if (request.ArtistIds is not null)
            {
                await UpdateTrackArtistsAsync(track, request.ArtistIds, cancellationToken);
            }
            if (request.TagSlugs is not null)
            {
                await UpdateTrackTagsAsync(track, request.TagSlugs, cancellationToken);
            }
        }

        return track.ToValueResult();
    }

    public IQueryable<Track> GetQueryable()
    {
        return _dbContext.Tracks.AsNoTracking();
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalServerError(
                $"Cannot save changes to database: {ex.Message}"
            ).ToResult();
        }
    }

    private async Task<Result> UpdateTrackArtistsAsync(
        Track track,
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken
    )
    {
        if (track?.Artists is null || artistIds is null)
        {
            return new InternalServerError(
                "Couldn't update artist users"
            ).ToResult();
        }

        try
        {
            var artists = await _dbContext.Artists
                .AsNoTracking()
                .Where(a => artistIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            track.Artists.Clear();
            foreach (var artist in artists)
            {
                track.Artists.Add(artist);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalServerError(
                $"Couldn't update track artists: {ex.Message}"
            ).ToResult();
        }
    }

    private async Task<Result> UpdateTrackTagsAsync(
        Track track,
        IEnumerable<string> tagSlugs,
        CancellationToken cancellationToken
    )
    {
        if (track?.Tags is null || tagSlugs is null)
        {
            return new InternalServerError(
                "Couldn't update artist users"
            ).ToResult();
        }

        try
        {
            var tags = await _dbContext.Tags
                .Where(t => tagSlugs.Contains(t.Slug))
                .ToListAsync(cancellationToken);

            track.Tags.Clear();
            foreach (var tag in tags)
            {
                track.Tags.Add(tag);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalServerError(
                $"Couldn't update track tags: {ex.Message}"
            ).ToResult();
        }
    }
}