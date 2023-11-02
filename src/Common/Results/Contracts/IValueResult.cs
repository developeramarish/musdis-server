namespace Results.Contracts;

/// <summary>
/// Represents the result of an operation, indicating success or failure, with an associated value of type <typeparamref name="TValue" />.
/// </summary>
/// <typeparam name="TValue">The type of the associated value.</typeparam>
internal interface IValueResult<TValue> : IResult
{
    /// <summary>
    /// Gets the associated value when the result represents a successful outcome; otherwise, returns null.
    /// </summary>
    TValue? Value { get; }
}