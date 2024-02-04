using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="ITagService"/>
public sealed class TagService : ITagService
{
    private readonly IMusicServiceDbContext _dbContext;
    public TagService(IMusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<Result<Tag>> CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<Tag>> UpdateAsync(
        Guid id,
        UpdateTagRequest request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO Add validator

        var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        throw new NotImplementedException();
    }
    public async Task<Result> DeleteAsync(Guid tagId, CancellationToken cancellationToken = default)
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId, cancellationToken);
        if (tag is null)
        {
            return new NoContentError(
                $"Couldn't delete tag with id = {tagId} because it is not found."
            ).ToResult();
        }

        _dbContext.Tags.Remove(tag);

        var savingResult = await SaveChangesAsync(cancellationToken);
        return savingResult.IsSuccess
            ? Result.Success()
            : savingResult.Error.ToResult();
    }

    public IQueryable<Tag> GetQueryable()
    {
        return _dbContext.Tags.AsNoTracking().AsQueryable();
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