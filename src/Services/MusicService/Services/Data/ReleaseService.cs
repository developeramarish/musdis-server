using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

public sealed class ReleaseService : IReleaseService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateReleaseRequest> _createValidator;
    public ReleaseService(
        MusicServiceDbContext dbContext,
        IValidator<CreateReleaseRequest> createValidator,
        ISlugGenerator slugGenerator
    )
    {
        _dbContext = dbContext;
        _createValidator = createValidator;
        _slugGenerator = slugGenerator;
    }

    public async Task<Result<Release>> CreateAsync(
        CreateReleaseRequest request, 
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Cannot create Release, incorrect data!",
                validationResult.Errors.Select(e => e.ErrorMessage)
            ).ToValueResult<Release>();
        }

        var releaseType = await _dbContext.ReleaseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Slug == request.ReleaseTypeSlug, cancellationToken);
        if (releaseType is null)
        {
            return new ValidationError(
                "Cannot create Release, ReleaseTypeSlug is invalid."
            ).ToValueResult<Release>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Release>(request.Name, cancellationToken);
        if (slugResult.IsFailure)
        {
            return new InternalServerError(
                "Cannot generate slug for Release while creating."
            ).ToValueResult<Release>();
        }

        var release = new Release
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ReleaseTypeId = releaseType.Id,
            Slug = slugResult.Value,
            ReleaseDate = DateTime.Parse(request.ReleaseDate),
            CoverUrl = request.CoverUrl
        };

        throw new NotImplementedException();
    }

    private async Task<Result> AddReleaseArtists(
        Release release,
        IEnumerable<Guid> artistIds
    )
    {
        throw new NotImplementedException();
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
                $"Could not able to delete artist, content with Id={releaseId} not found."
            ).ToResult();
        }

        _dbContext.Releases.Remove(release);

        return Result.Success();
    }

    public Task<Result<Release>> UpdateAsync(
        Guid id, 
        UpdateReleaseRequest request, 
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
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
}