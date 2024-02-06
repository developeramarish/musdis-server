using System.Text;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

using Slugify;

namespace Musdis.MusicService.Services;

/// <inheritdoc cref="ISlugGenerator"/>
public class SlugGenerator : ISlugGenerator
{
    private readonly ISlugHelper _slugHelper;
    private readonly IMusicServiceDbContext _dbContext;

    public SlugGenerator(
        ISlugHelper slugHelper,
        IMusicServiceDbContext dbContext
    )
    {
        _slugHelper = slugHelper;
        _dbContext = dbContext;
    }

    public Result<string> Generate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty.ToValueResult();
        }

        try
        {
            var slug = _slugHelper.GenerateSlug(value);

            return slug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new Error($"Could not create slug : {ex.Message}")
                .ToValueResult<string>();
        }
    }

    public async Task<Result<string>> GenerateUniqueSlugAsync<TModel>(
        string value,
        CancellationToken cancellationToken = default
    ) where TModel : class
    {
        var slugResult = Generate(value);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<string>();
        }

        var slug = slugResult.Value;

        try
        {
            var tableName = $"{typeof(TModel).Name}s";
            var similarSlugs = await _dbContext
                .SqlQuery<string>($"SELECT [Slug] FROM [{tableName}]")
                .Where(s => s.StartsWith(slug))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var suffixedSlug = slug;
            // Adding number suffix until it is unique
            for (var i = 1; similarSlugs.Contains(suffixedSlug); i++)
            {
                suffixedSlug = slug + '-' + i;
            }

            return suffixedSlug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new Error(
                $"Could not generate slug!: {ex.Message}"
            ).ToValueResult<string>();
        }
    }
}