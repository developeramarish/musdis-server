namespace Musdis.OperationResults;

/// <summary>
///     Represents an error in Results pattern.
/// </summary>
public class Error
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Error"/> class with the specified description.
    /// </summary>
    /// 
    /// <param name="description">The description of the error.</param>
    public Error(string description)
    {
        Description = description;
    }

    /// <summary>
    /// The description of the error.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Converts an error into a failed <see cref="Result"/> with the provided error.
    /// </summary>
    /// 
    /// <returns>
    ///     A new failed <see cref="Result"/> instance with the specified error.
    /// </returns>
    public Result ToResult()
    {
        return Result.Failure(this);
    }

    /// <summary>
    ///     Converts an error into a failed <see cref="Result{TValue}"/> with the provided error.
    /// </summary>
    /// 
    /// <typeparam name="TValue">The type of the associated value.</typeparam>
    /// 
    /// <returns>
    ///     A new failed <see cref="Result{TValue}"/> instance with the specified error.
    /// </returns>
    public Result<TValue> ToValueResult<TValue>()
    {
        return Result<TValue>.Failure(this);
    }
}