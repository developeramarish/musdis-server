using System.IdentityModel.Tokens.Jwt;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="ITrackService"/>
public sealed class TrackService : ITrackService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateTrackRequest> _createRequestValidator;
    private readonly IValidator<UpdateTrackRequest> _updateRequestValidator;
    private readonly IValidator<CreateReleaseRequest.TrackInfo> _trackInfoValidator;

    public TrackService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateTrackRequest> createRequestValidator,
        IValidator<UpdateTrackRequest> updateRequestValidator,
        IValidator<CreateReleaseRequest.TrackInfo> trackInfoValidator,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
        _trackInfoValidator = trackInfoValidator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<TrackDto>> CreateForReleaseAsync(
        CreateReleaseRequest.TrackInfo trackInfo,
        Release release,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userId is null)
        {
            return new UnauthorizedError(
                "Cannot create a Track without a valid User"
            ).ToValueResult<TrackDto>();
        }

        var validationResult = await _trackInfoValidator.ValidateAsync(trackInfo, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create a Track, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<TrackDto>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Track>(
            trackInfo.Title,
            cancellationToken
        );

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<TrackDto>();
        }

        var track = new Track
        {
            Id = Guid.NewGuid(),
            Title = trackInfo.Title,
            Slug = slugResult.Value,
            ReleaseId = release.Id,
            CreatorId = userId,
            AudioUrl = trackInfo.AudioFile.Url,
            AudioFileId = trackInfo.AudioFile.Id,
            CoverUrl = release.CoverUrl
        };

        var artistIds = trackInfo.ArtistIds;
        if (artistIds is null)
        {
            if (release.Artists is null)
            {
                await _dbContext.Entry(release).Collection(r => r.Artists!).LoadAsync(cancellationToken);
            }
            artistIds = release.Artists!.Select(a => a.Id);
        }

        var addArtistsResult = await AddTrackArtistsAsync(track, artistIds, cancellationToken);
        if (addArtistsResult.IsFailure)
        {
            return addArtistsResult.Error.ToValueResult<TrackDto>();
        }

        var addTagsResult = await AddTrackTagsAsync(track, trackInfo.TagSlugs, cancellationToken);
        if (addTagsResult.IsFailure)
        {
            return addTagsResult.Error.ToValueResult<TrackDto>();
        }

        await _dbContext.Tracks.AddAsync(track, cancellationToken);

        await _dbContext.Entry(track).Reference(t => t.Release).LoadAsync(cancellationToken);

        return TrackDto.FromTrack(track).ToValueResult();
    }

    public async Task<Result<TrackDto>> CreateAsync(
        CreateTrackRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userId is null)
        {
            return new UnauthorizedError(
                "Cannot create a Track without a valid User"
            ).ToValueResult<TrackDto>();
        }

        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create a Track, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<TrackDto>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Track>(
            request.Title,
            cancellationToken
        );
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<TrackDto>();
        }

        var release = await _dbContext.Releases
            .FirstOrDefaultAsync(r => r.Id == request.ReleaseId, cancellationToken);
        if (release is null)
        {
            return new ValidationError(
                "Could not create a Track, invalid Release!"
            ).ToValueResult<TrackDto>();
        }

        var track = new Track
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = slugResult.Value,
            ReleaseId = request.ReleaseId,
            CreatorId = userId,
            AudioUrl = request.AudioFile.Url,
            AudioFileId = request.AudioFile.Id,
            CoverUrl = release.CoverUrl
        };

        var addArtistsResult = await AddTrackArtistsAsync(track, request.ArtistIds, cancellationToken);
        if (addArtistsResult.IsFailure)
        {
            return addArtistsResult.Error.ToValueResult<TrackDto>();
        }

        var addTagsResult = await AddTrackTagsAsync(track, request.TagSlugs, cancellationToken);
        if (addTagsResult.IsFailure)
        {
            return addTagsResult.Error.ToValueResult<TrackDto>();
        }

        await _dbContext.Tracks.AddAsync(track, cancellationToken);

        await _dbContext.Entry(track).Reference(t => t.Release).LoadAsync(cancellationToken);

        return TrackDto.FromTrack(track).ToValueResult();
    }

    private async Task<Result> AddTrackTagsAsync(
        Track track,
        IEnumerable<string> tagSlugs,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var tags = await _dbContext.Tags
                .Where(t => tagSlugs.Contains(t.Slug))
                .ToListAsync(cancellationToken);

            track.Tags = [];
            foreach (var tag in tags)
            {
                track.Tags.Add(tag);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> AddTrackArtistsAsync(
        Track track,
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var artists = await _dbContext.Artists
                .Where(a => artistIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            track.Artists = [];
            foreach (var artist in artists)
            {
                track.Artists.Add(artist);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
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
                $"Cannot delete, track with Id = {{{trackId}}} not found"
            ).ToResult();
        }

        _dbContext.Tracks.Remove(track);

        return Result.Success();
    }

    public async Task<Result<TrackDto>> UpdateAsync(
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
            ).ToValueResult<TrackDto>();
        }

        var track = await _dbContext.Tracks
            .Include(t => t.Tags)
            .Include(t => t.Artists)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (track is null)
        {
            return new NotFoundError(
                $"Cannot update track with Id = {{{id}}}, it is not found."
            ).ToValueResult<TrackDto>();
        }

        if (request.Title is not null)
        {
            var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Track>(
                request.Title,
                cancellationToken
            );
            if (slugResult.IsFailure)
            {
                return slugResult.Error.ToValueResult<TrackDto>();
            }

            track.Title = request.Title;
            track.Slug = slugResult.Value;
        }

        if (request.ArtistIds is not null)
        {
            var result = await UpdateTrackArtistsAsync(track, request.ArtistIds, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ToValueResult<TrackDto>();
            }
        }

        if (request.TagSlugs is not null)
        {
            var result = await UpdateTrackTagsAsync(track, request.TagSlugs, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ToValueResult<TrackDto>();
            }
        }

        return TrackDto.FromTrack(track).ToValueResult();
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
