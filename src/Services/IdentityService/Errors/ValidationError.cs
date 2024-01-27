using FluentValidation.Results;

using Musdis.OperationResults;

namespace Musdis.IdentityService.Errors;


/// <summary>
/// Represents an error indicating a validation failure, 
/// associated with HTTP status code 400 (Bad Request).
/// </summary>
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
    /// Failures caused error.
    /// </summary>
    public IEnumerable<ValidationFailure> Failures { get; init; }
}