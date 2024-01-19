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
        : base(400, description)
    {
        Failures = null;
    }

    public ValidationError(
        string description,
        IEnumerable<ValidationFailure> failures
    ) : base(400, description)
    {
        Failures = failures;
    }
    public IEnumerable<ValidationFailure>? Failures { get; init; }
}