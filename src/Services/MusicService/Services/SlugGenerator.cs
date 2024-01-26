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
            var combined = string.Join(' ', GetCombinedValues());
            var slug = _slugHelper.GenerateSlug(combined);

            return slug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new Error(500, $"Could not create slug : {ex.Message}")
                .ToValueResult<string>();
        }

        // Combine value and values to one enumerable. 
        IEnumerable<string> GetCombinedValues()
        {
            yield return value;

            foreach (var v in additionalValues)
            {
                yield return v;
            }
        }

        // Basic realization.
        // var stringBuilder = new StringBuilder();
        // try
        // {
        //     foreach (var val in GetValues())
        //     {
        //         var part = Regex.Replace(val, "[^A-Za-z0-9]", " ");
        //         part = Regex.Replace(part, @"\s+", " ").Trim();
        //         part = part.Replace(" ", "-");
        //         part = part.ToLowerInvariant();

        //         stringBuilder.Append(part);
        //     }

        //     return stringBuilder.ToString().ToValueResult();
        // }
        // catch (Exception exc)
        // {
        //     return new Error(500, $"Could not create : {exc.Message}")
        //         .ToValueResult<string>();
        // }
    }
}