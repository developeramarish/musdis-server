using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="IArtistTypeService"/>.
public sealed class ArtistTypeService : IArtistTypeService
{
    private readonly IMusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;

    public ArtistTypeService(
        IMusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
    }

    // TODO add validator
    public async Task<Result<ArtistType>> CreateAsync(
        CreateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var slugResult = _slugGenerator.Generate(request.Name);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ArtistType>();
        }

        var artistType = new ArtistType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = slugResult.Value,
        };

        await _dbContext.ArtistTypes.AddAsync(artistType, cancellationToken);

        var savingResult = await SaveChangesAsync(cancellationToken);
        return savingResult.IsSuccess
            ? artistType.ToValueResult()
            : savingResult.Error.ToValueResult<ArtistType>();
    }

    public async Task<Result> DeleteAsync(Guid artistTypeId, CancellationToken cancellationToken = default)
    {
        var artistType = await _dbContext.ArtistTypes
            .FirstOrDefaultAsync(at => at.Id == artistTypeId, cancellationToken);

        if (artistType is null)
        {
            return new NoContentError(
                $"Couldn't delete ArtistType, content with Id={artistTypeId} not found."
            ).ToResult();
        }

        _dbContext.ArtistTypes.Remove(artistType);

        var savingResult = await SaveChangesAsync(cancellationToken);
        return savingResult.IsSuccess
            ? Result.Success()
            : savingResult.Error.ToResult();
    }

    // TODO: add validator
    public async Task<Result<ArtistType>> UpdateAsync(
        Guid id,
        UpdateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var artistType = await _dbContext.ArtistTypes
            .FirstOrDefaultAsync(at => at.Id == id, cancellationToken);

        if (artistType is null)
        {
            return new NotFoundError(
                $"Cannot update artist type, content with Id={id} not found."
            ).ToValueResult<ArtistType>();
        }

        artistType.Name = request.Name;

        var slugResult = _slugGenerator.Generate(request.Name);
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ArtistType>();
        }

        artistType.Slug = slugResult.Value;

        var savingResult = await SaveChangesAsync(cancellationToken);
        return savingResult.IsSuccess
            ? artistType.ToValueResult()
            : savingResult.Error.ToValueResult<ArtistType>();
    }

    public IQueryable<ArtistType> GetQueryable()
    {
        return _dbContext.ArtistTypes.AsNoTracking().AsQueryable();
    }

    private async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
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