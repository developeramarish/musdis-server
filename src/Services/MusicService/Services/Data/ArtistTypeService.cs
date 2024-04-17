using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="IArtistTypeService"/>.
public sealed class ArtistTypeService : IArtistTypeService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateArtistTypeRequest> _createRequestValidator;
    private readonly IValidator<UpdateArtistTypeRequest> _updateRequestValidator;

    public ArtistTypeService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateArtistTypeRequest> createRequestValidator,
        IValidator<UpdateArtistTypeRequest> updateRequestValidator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<Result<ArtistTypeDto>> CreateAsync(
        CreateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToError(
                "Cannot update an ArtistType, incorrect data."
            ).ToValueResult<ArtistTypeDto>();
        }

        var slugResult = _slugGenerator.Generate(request.Name);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ArtistTypeDto>();
        }

        var artistType = new ArtistType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = slugResult.Value,
        };

        await _dbContext.ArtistTypes.AddAsync(artistType, cancellationToken);

        return ArtistTypeDto.FromArtistType(artistType).ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid artistTypeId, CancellationToken cancellationToken = default)
    {
        var artistType = await _dbContext.ArtistTypes
            .FirstOrDefaultAsync(at => at.Id == artistTypeId, cancellationToken);

        if (artistType is null)
        {
            return new NoContentError(
                $"Cannot delete ArtistType, content with Id = {{{artistTypeId}}} not found."
            ).ToResult();
        }

        _dbContext.ArtistTypes.Remove(artistType);

        return Result.Success();
    }

    public async Task<Result<ArtistTypeDto>> UpdateAsync(
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
                $"Cannot update ArtistType, content with Id = {{{id}}} not found."
            ).ToValueResult<ArtistTypeDto>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToError(
                "Cannot update an ArtistType, incorrect data."
            ).ToValueResult<ArtistTypeDto>();
        }


        var slugResult = _slugGenerator.Generate(request.Name);
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ArtistTypeDto>();
        }

        artistType.Name = request.Name;
        artistType.Slug = slugResult.Value;

        return ArtistTypeDto.FromArtistType(artistType).ToValueResult();
    }

    public IQueryable<ArtistType> GetQueryable()
    {
        return _dbContext.ArtistTypes.AsNoTracking();
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