namespace Results.Contracts;

/// <summary>
/// Represents the result of an operation, indicating success or failure.
/// </summary>
internal interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed outcome.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Gets the error associated with the result, null if succeed result.
    /// </summary>
    Error? Error { get; }
}