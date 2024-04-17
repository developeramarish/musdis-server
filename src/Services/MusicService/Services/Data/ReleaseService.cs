using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Exceptions;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

public sealed class ReleaseService : IReleaseService
{
    private readonly ITrackService _trackService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateReleaseRequest> _createValidator;
    private readonly IValidator<UpdateReleaseRequest> _updateValidator;

    public ReleaseService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        ITrackService trackService,
        IValidator<CreateReleaseRequest> createValidator,
        IValidator<UpdateReleaseRequest> updateValidator,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _trackService = trackService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ReleaseDto>> CreateAsync(
        CreateReleaseRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userId is null)
        {
            return new UnauthorizedError(
                "Cannot create a Release without a valid User"
            ).ToValueResult<ReleaseDto>();
        }

        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Cannot create Release, incorrect data!",
                validationResult.Errors.Select(e => e.ErrorMessage)
            ).ToValueResult<ReleaseDto>();
        }

        var releaseType = await _dbContext.ReleaseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Slug == request.ReleaseTypeSlug, cancellationToken);
        if (releaseType is null)
        {
            return new ValidationError(
                "Cannot create Release, ReleaseTypeSlug is invalid."
            ).ToValueResult<ReleaseDto>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Release>(request.Name, cancellationToken);
        if (slugResult.IsFailure)
        {
            return new InternalServerError(
                "Cannot generate slug for Release while creating."
            ).ToValueResult<ReleaseDto>();
        }

        var release = new Release
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ReleaseTypeId = releaseType.Id,
            Slug = slugResult.Value,
            ReleaseDate = DateTime.SpecifyKind(
                DateTime.Parse(request.ReleaseDate, CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            ),
            CoverUrl = request.CoverFile.Url,
            CoverFileId = request.CoverFile.Id,
            CreatorId = userId
        };

        var addArtistsResult = await AddReleaseArtistsAsync(release, request.ArtistIds, cancellationToken);
        if (addArtistsResult.IsFailure)
        {
            return addArtistsResult.Error.ToValueResult<ReleaseDto>();
        }

        var addTracksResult = await AddReleaseTracksAsync(release, request.Tracks, cancellationToken);
        if (addTracksResult.IsFailure)
        {
            return addTracksResult.Error.ToValueResult<ReleaseDto>();
        }

        await _dbContext.Releases.AddAsync(release, cancellationToken);

        await _dbContext.Entry(release).Reference(r => r.ReleaseType).LoadAsync(cancellationToken);
        await _dbContext.Entry(release).Collection(r => r.Tracks!).LoadAsync(cancellationToken);

        return ReleaseDto.FromRelease(release).ToValueResult();
    }

    public async Task<Result> DeleteAsync(
        Guid releaseId,
        CancellationToken cancellationToken = default
    )
    {
        var release = await _dbContext.Releases
            .FirstOrDefaultAsync(a => a.Id == releaseId, cancellationToken);
        if (release is null)
        {
            return new NoContentError(
                $"Could not able to delete artist, content with Id = {{{releaseId}}} not found."
            ).ToResult();
        }

        _dbContext.Releases.Remove(release);

        return Result.Success();
    }

    public async Task<Result<ReleaseDto>> UpdateAsync(
        Guid id,
        UpdateReleaseRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Cannot update release, incorrect data",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<ReleaseDto>();
        }

        var release = await _dbContext.Releases
            .Include(r => r.ReleaseType)
            .Include(r => r.Artists)
            .Include(r => r.Tracks)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (release is null)
        {
            return new NotFoundError(
                $"Release with Id = {{{id}}} is not found."
            ).ToValueResult<ReleaseDto>();
        }

        if (request.Name is not null)
        {
            var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Release>(
                request.Name,
                cancellationToken
            );
            if (slugResult.IsFailure)
            {
                return slugResult.Error.ToValueResult<ReleaseDto>();
            }

            release.Name = request.Name;
            release.Slug = slugResult.Value;
        }
        if (request.ReleaseTypeSlug is not null)
        {
            var releaseType = await _dbContext.ReleaseTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Slug == request.ReleaseTypeSlug, cancellationToken);
            if (releaseType is null)
            {
                return new InternalServerError(
                    $"Cannot update Release, ReleaseType with Slug = {{{request.ReleaseTypeSlug}}} is not found."
                ).ToValueResult<ReleaseDto>();
            }

            release.ReleaseTypeId = releaseType.Id;
        }

        if (request.ReleaseDate is not null)
        {
            release.ReleaseDate = DateTime.Parse(request.ReleaseDate, CultureInfo.InvariantCulture);
        }

        if (request.CoverFile is not null)
        {
            release.CoverUrl = request.CoverFile.Url;
            release.CoverFileId = request.CoverFile.Id;
        }
        if (request.ArtistIds is not null)
        {
            var updateArtistsResult = await UpdateReleaseArtistsAsync(
                release,
                request.ArtistIds,
                cancellationToken
            );
            if (updateArtistsResult.IsFailure)
            {
                return updateArtistsResult.Error.ToValueResult<ReleaseDto>();
            }
        }

        return ReleaseDto.FromRelease(release).ToValueResult();
    }

    public IQueryable<Release> GetQueryable()
    {
        return _dbContext.Releases.AsNoTracking();
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

    // Helper methods
    private async Task<Result> AddReleaseTracksAsync(
        Release release,
        IEnumerable<CreateReleaseRequest.TrackInfo> trackInfos,
        CancellationToken cancellationToken
    )
    {
        if (release?.Artists is null)
        {
            throw new InvalidMethodCallException("Cannot access Artists property in creating Release");
        }

        try
        {
            foreach (var trackInfo in trackInfos)
            {
                var result = await _trackService.CreateForReleaseAsync(
                    trackInfo,
                    release,
                    cancellationToken
                );
                if (result.IsFailure)
                {
                    return result.Error.ToResult();
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> AddReleaseArtistsAsync(
        Release release,
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var artists = await _dbContext.Artists
                .Where(a => artistIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            release.Artists = [];
            foreach (var artist in artists)
            {
                release.Artists.Add(artist);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> UpdateReleaseArtistsAsync(
        Release release,
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken
    )
    {
        if (release?.Artists is null || artistIds is null)
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

            release.Artists.Clear();
            foreach (var artist in artists)
            {
                release.Artists.Add(artist);
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
}
