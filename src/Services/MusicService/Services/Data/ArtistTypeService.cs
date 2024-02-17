using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
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

    public async Task<Result<ArtistType>> CreateAsync(
        CreateArtistTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var existingArtistType = await _dbContext.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);
        if (existingArtistType is not null)
        {
            return new ConflictError(
                $"Artist type with name = {request.Name} exists"
            ).ToValueResult<ArtistType>();
        }

        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create an ArtistType, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<ArtistType>();
        }

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

        return artistType.ToValueResult();
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

        return Result.Success();
    }

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
                $"Cannot update ArtistType, content with Id={id} not found."
            ).ToValueResult<ArtistType>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not update an ArtistType, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<ArtistType>();
        }


        var slugResult = _slugGenerator.Generate(request.Name);
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ArtistType>();
        }

        artistType.Name = request.Name;
        artistType.Slug = slugResult.Value;

        return artistType.ToValueResult();
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