using System.Text;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

using Slugify;

namespace Musdis.MusicService.Services.Utils;

/// <inheritdoc cref="ISlugGenerator"/>
public class SlugGenerator : ISlugGenerator
{
    private readonly ISlugHelper _slugHelper;
    private readonly MusicServiceDbContext _dbContext;

    public SlugGenerator(
        ISlugHelper slugHelper,
        MusicServiceDbContext dbContext
    )
    {
        _slugHelper = slugHelper;
        _dbContext = dbContext;
    }

    public Result<string> Generate(string value)
    {
        try
        {
            var slug = _slugHelper.GenerateSlug(value);

            return slug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new Error($"Cannot create a slug: {ex.Message}")
                .ToValueResult<string>();
        }
    }

    public async Task<Result<string>> GenerateUniqueSlugAsync<TEntity>(
        string value,
        CancellationToken cancellationToken = default
    ) where TEntity : class
    {
        var slugResult = Generate(value);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<string>();
        }

        var slug = slugResult.Value;

        var similarSlugsResult = await GetSimilarSlugAsync<TEntity>(slug, cancellationToken);
        if (similarSlugsResult.IsFailure)
        {
            return similarSlugsResult.Error.ToValueResult<string>();
        }

        var suffixedSlug = slug;
        // Adding number suffix until it is unique
        for (var i = 1; similarSlugsResult.Value.Contains(suffixedSlug); i++)
        {
            suffixedSlug = slug + '-' + i;
        }

        return suffixedSlug.ToValueResult();
    }

    /// <summary>
    ///     Gets slugs that starts with <paramref name="slug"/> 
    ///     for <typeparamref name="TEntity"/> by type checking 
    ///     and mapping to related <see cref="IMusicServiceDbContext"/> DbSet.
    /// </summary>
    /// <remarks>
    ///     Supported types: <see cref="Artist"/>, <see cref="ArtistType"/>, 
    ///     <see cref="Release"/>, <see cref="ReleaseType"/>, <see cref="Tag"/>, <see cref="Track"/>.
    /// </remarks>
    /// 
    /// <typeparam name="TEntity">
    ///     Type of the entity to get similar slugs for.
    /// </typeparam>
    /// <param name="slug">
    ///     Slug to compare.
    /// </param>
    /// <returns>
    ///     A task representing asynchronous operation which contains
    ///     the result of operation with collection of similar slugs.
    /// </returns>
    private async Task<Result<List<string>>> GetSimilarSlugAsync<TEntity>(
        string slug,
        CancellationToken cancellationToken
    )
    {
        IQueryable<string> queryable;
        var type = typeof(TEntity);

        if (type == typeof(Artist))
        {
            queryable = _dbContext.Artists
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else if (type == typeof(ArtistType))
        {
            queryable = _dbContext.ArtistTypes
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else if (type == typeof(Release))
        {
            queryable = _dbContext.Releases
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else if (type == typeof(ReleaseType))
        {
            queryable = _dbContext.ReleaseTypes
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else if (type == typeof(Tag))
        {
            queryable = _dbContext.Tags
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else if (type == typeof(Track))
        {
            queryable = _dbContext.Tracks
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug);
        }
        else
        {
            return new Error(
                $"Cannot generate unique slug for type {type.Name}"
            ).ToValueResult<List<string>>();
        }

        var result = await queryable.ToListAsync(cancellationToken);

        return result.ToValueResult();
    }
}