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
    /// 
    /// <returns>
    ///     The <see cref="Result{TValue}"/> of operation, which contains generated slug.
    /// </returns>
    Result<string> Generate(string value);

    /// <summary>
    ///     Generates unique slug for the <typeparamref name="TModel"/> model.
    /// </summary>
    /// 
    /// <param name="value">
    ///     The string to generate the slug for.
    /// </param>
    /// <param name="cancellationToken">
    ///     The token for the cancellation.
    /// </param>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// 
    /// <returns>
    ///     The result object that contains a string value which is a generated slug.
    /// </returns>
    Task<Result<string>> GenerateUniqueSlugAsync<TModel>(
        string value,
        CancellationToken cancellationToken = default
    ) where TModel : class;
}