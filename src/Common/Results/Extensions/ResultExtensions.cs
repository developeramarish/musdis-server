namespace Results.Extensions;

/// <summary>
/// Provides extension methods for working with results and errors.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts an error into a failed <see cref="Result"/> with the provided error.
    /// </summary>
    /// <param name="error">The error to be associated with the failed result.</param>
    /// <returns>A new failed <see cref="Result"/> instance with the specified error.</returns>
    public static Result ToResult(this Error error)
    {
        return Result.Failure(error);
    }

        /// <summary>
    /// Converts a value into a successful <see cref="Result{TValue}"/> with the provided value.
    /// </summary>
    /// <typeparam name="TValue">The type of the associated value.</typeparam>
    /// <param name="value">The value to be wrapped in a successful result.</param>
    /// <returns>A new successful <see cref="Result{TValue}"/> instance with the specified value.</returns>
    public static Result<TValue> ToValueResult<TValue>(this TValue value)
    {
        return Result<TValue>.Success(value);
    }

    /// <summary>
    /// Converts an error into a failed <see cref="Result{TValue}"/> with the provided error.
    /// </summary>
    /// <typeparam name="TValue">The type of the associated value.</typeparam>
    /// <param name="error">The error to be associated with the failed result.</param>
    /// <returns>A new failed <see cref="Result{TValue}"/> instance with the specified error.</returns>
    public static Result<TValue> ToValueResult<TValue>(this Error error)
    {
        return Result<TValue>.Failure(error);
    }
}