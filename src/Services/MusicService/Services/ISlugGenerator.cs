using Musdis.OperationResults;

namespace Musdis.MusicService.Services;

/// <summary>
///     Utility service for generating human-readable identifiers.
/// </summary>
public interface ISlugGenerator
{
    /// <summary>
    ///     Generates a slug.
    /// </summary>
    /// 
    /// <param name="value">
    ///     First value from which to generate a slug.
    /// </param>
    /// <param name="additionalValues">
    ///     Additional values to generate a slug.
    /// </param>
    /// 
    /// <returns>
    ///     The <see cref="Result{TValue}"/> of operation, which contains generated slug.
    /// </returns>
    Result<string> Generate(string value, params string[] additionalValues);
}