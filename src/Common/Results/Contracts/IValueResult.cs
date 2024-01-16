namespace Results.Contracts;

/// <summary>
/// Represents the result of an operation, indicating success or failure, 
/// with an associated value of type <typeparamref name="TValue" />.
/// </summary>
/// <typeparam name="TValue">The type of the associated value.</typeparam>
internal interface IValueResult<out TValue> : IResult
{
    /// <summary>
    /// The value of the result. default if result is failed.
    /// </summary>
    TValue? Value { get; }
}