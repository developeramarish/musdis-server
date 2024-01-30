namespace Musdis.OperationResults.Extensions;

/// <summary>
///     Provides extension methods for working with results and errors.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts a value into a successful <see cref="Result{TValue}"/> with the provided value.
    /// </summary>
    /// 
    /// <typeparam name="TValue">The type of the associated value.</typeparam>
    /// <param name="value">
    ///     The value to be wrapped in a successful result.
    /// </param>
    /// 
    /// <returns>
    ///     A new successful <see cref="Result{TValue}"/> instance with the specified value.
    /// </returns>
    public static Result<TValue> ToValueResult<TValue>(this TValue value)
    {
        return Result<TValue>.Success(value);
    }
}