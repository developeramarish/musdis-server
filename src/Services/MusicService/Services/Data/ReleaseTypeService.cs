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

/// <inheritdoc cref="IReleaseTypeService"/>
public sealed class ReleaseTypeService : IReleaseTypeService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateReleaseTypeRequest> _createRequestValidator;
    private readonly IValidator<UpdateReleaseTypeRequest> _updateRequestValidator;

    public ReleaseTypeService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateReleaseTypeRequest> createRequestValidator,
        IValidator<UpdateReleaseTypeRequest> updateRequestValidator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<Result<ReleaseTypeDto>> CreateAsync(
        CreateReleaseTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var existingReleaseType = await _dbContext.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);
        if (existingReleaseType is not null)
        {
            return new ConflictError(
                $"Release type with Name = {{{request.Name}}} exists"
            ).ToValueResult<ReleaseTypeDto>();
        }

        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create an ReleaseType, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<ReleaseTypeDto>();
        }

        var slugResult = _slugGenerator.Generate(request.Name);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ReleaseTypeDto>();
        }

        var releaseType = new ReleaseType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = slugResult.Value,
        };

        await _dbContext.ReleaseTypes.AddAsync(releaseType, cancellationToken);

        return ReleaseTypeDto.FromReleaseType(releaseType).ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid releaseTypeId, CancellationToken cancellationToken = default)
    {
        var releaseType = await _dbContext.ReleaseTypes
            .FirstOrDefaultAsync(at => at.Id == releaseTypeId, cancellationToken);

        if (releaseType is null)
        {
            return new NoContentError(
                $"Couldn't delete ReleaseType, content with Id = {{{releaseTypeId}}} not found."
            ).ToResult();
        }

        _dbContext.ReleaseTypes.Remove(releaseType);

        return Result.Success();
    }

    public async Task<Result<ReleaseTypeDto>> UpdateAsync(
        Guid releaseTypeId,
        UpdateReleaseTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var releaseType = await _dbContext.ReleaseTypes
            .FirstOrDefaultAsync(at => at.Id == releaseTypeId, cancellationToken);

        if (releaseType is null)
        {
            return new NotFoundError(
                $"Cannot update ReleaseType, content with Id = {{{releaseTypeId}}} not found."
            ).ToValueResult<ReleaseTypeDto>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not update an ReleaseType, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<ReleaseTypeDto>();
        }


        var slugResult = _slugGenerator.Generate(request.Name);
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<ReleaseTypeDto>();
        }

        releaseType.Name = request.Name;
        releaseType.Slug = slugResult.Value;

        return ReleaseTypeDto.FromReleaseType(releaseType).ToValueResult();
    }

    public IQueryable<ReleaseType> GetQueryable()
    {
        return _dbContext.ReleaseTypes.AsNoTracking();
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