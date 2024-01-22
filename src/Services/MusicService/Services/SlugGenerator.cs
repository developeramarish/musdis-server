using System.Text;
using System.Text.RegularExpressions;

using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.MusicService.Services;

/// <inheritdoc cref="ISlugGenerator"/>
public class SlugGenerator : ISlugGenerator
{
    public Result<string> Generate(string value, params string[] additionalValues)
    {
        if (string.IsNullOrWhiteSpace(value) && additionalValues.Length == 0)
        {
            return string.Empty.ToValueResult();
        }

        // Combine value and values to one enumerable. 
        IEnumerable<string> GetValues()
        {
            yield return value;

            for (var i = 0; i < additionalValues.Length; i++)
            {
                yield return additionalValues[i];
            }
        }

        var stringBuilder = new StringBuilder();
        try
        {
            foreach (var val in GetValues())
            {
                var part = Regex.Replace(val, "[^A-Za-z0-9]", " ");
                part = Regex.Replace(part, @"\s+", " ").Trim();
                part = part.Replace(" ", "-");
                part = part.ToLowerInvariant();

                stringBuilder.Append(part);
            }

            return stringBuilder.ToString().ToValueResult();
        }
        catch (Exception exc)
        {
            return new Error(500, $"Could not create : {exc.Message}")
                .ToValueResult<string>();
        }
    }
}