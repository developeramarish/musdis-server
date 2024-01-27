using System.Text;
using System.Text.RegularExpressions;

using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

using Slugify;

namespace Musdis.MusicService.Services;

/// <inheritdoc cref="ISlugGenerator"/>
public class SlugGenerator : ISlugGenerator
{
    private readonly ISlugHelper _slugHelper;

    public SlugGenerator(ISlugHelper slugHelper)
    {
        _slugHelper = slugHelper;
    }

    public Result<string> Generate(string value, params string[] additionalValues)
    {
        if (string.IsNullOrWhiteSpace(value) && additionalValues.Length == 0)
        {
            return string.Empty.ToValueResult();
        }

        try
        {
            string[] strings = [value, .. additionalValues];
            var combined = string.Join(' ', strings);
            var slug = _slugHelper.GenerateSlug(combined);

            return slug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new Error(500, $"Could not create slug : {ex.Message}")
                .ToValueResult<string>();
        }
    }
}