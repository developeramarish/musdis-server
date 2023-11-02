using Results;

namespace IdentityService.Errors;

/// <summary>
/// Represents an error indicating a validation failure, 
/// associated with HTTP status code 400 (Bad Request).
/// </summary>
public sealed class ValidationError : Error
{
    public ValidationError(string description)
        : base(400, description) { }
}