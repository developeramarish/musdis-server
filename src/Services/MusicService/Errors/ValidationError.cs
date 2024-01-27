using FluentValidation.Results;

using Musdis.OperationResults;

namespace Musdis.MusicService.Errors;

/// <summary>
///     Represents an error indicating a validation failure, 
///     associated with HTTP status code 400 (Bad Request).
/// </summary>
/// 
/// <param name="description">
///     The description of the error.
/// </param>
public sealed class ValidationError : Error
{
    public ValidationError(string description)
        : base(StatusCodes.Status400BadRequest, description)
    {
        Failures = [];
    }
    public ValidationError(
        string description,
        IEnumerable<ValidationFailure> failures
    ) : base(StatusCodes.Status400BadRequest, description)
    {
        Failures = failures;
    }

    /// <summary>
    ///     A collection of failures that caused error.
    /// </summary>
    public IEnumerable<ValidationFailure> Failures { get; init; }
}