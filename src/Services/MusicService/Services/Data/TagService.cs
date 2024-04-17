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

/// <inheritdoc cref="ITagService"/>
public sealed class TagService : ITagService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateTagRequest> _createRequestValidator;
    private readonly IValidator<UpdateTagRequest> _updateRequestValidator;
    public TagService(
        MusicServiceDbContext dbContext,
        IValidator<CreateTagRequest> createRequestValidator,
        IValidator<UpdateTagRequest> updateRequestValidator,
        ISlugGenerator slugGenerator
    )
    {
        _dbContext = dbContext;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
        _slugGenerator = slugGenerator;
    }
    public async Task<Result<TagDto>> CreateAsync(
        CreateTagRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToError(
                "Cannot create an Tag, incorrect data."
            ).ToValueResult<TagDto>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Tag>(
            request.Name,
            cancellationToken
        );
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<TagDto>();
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = slugResult.Value,
        };
        await _dbContext.Tags.AddAsync(tag, cancellationToken);

        return TagDto.FromTag(tag).ToValueResult();
    }
    public async Task<Result<TagDto>> UpdateAsync(
        Guid id,
        UpdateTagRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (tag is null)
        {
            return new NotFoundError(
                $"Cannot update tag, Tag with Id = {{{id}}} not found."
            ).ToValueResult<TagDto>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not update a Tag, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<TagDto>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Tag>(
            request.Name,
            cancellationToken
        );
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<TagDto>();
        }

        tag.Name = request.Name;
        tag.Slug = slugResult.Value;

        return TagDto.FromTag(tag).ToValueResult();
    }
    public async Task<Result> DeleteAsync(Guid tagId, CancellationToken cancellationToken = default)
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId, cancellationToken);
        if (tag is null)
        {
            return new NoContentError(
                $"Couldn't delete tag with Id = {{{tagId}}} because it is not found."
            ).ToResult();
        }

        _dbContext.Tags.Remove(tag);

        return Result.Success();
    }

    public IQueryable<Tag> GetQueryable()
    {
        return _dbContext.Tags.AsNoTracking();
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