namespace Musdis.OperationResults.Contracts;

/// <summary>
///     Represents the result of an operation, indicating success or failure.
/// </summary>
internal interface IResult
{
    /// <summary>
    ///     Is result succeed.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    ///     Is result failed.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    ///     The error associated with the result, null if succeed result.
    /// </summary>
    Error? Error { get; }
}