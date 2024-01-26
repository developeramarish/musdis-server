using Musdis.OperationResults;

namespace Musdis.IdentityService.Errors;

/// <summary>
/// Represents an error indicating an internal server error, 
/// associated with HTTP status code 500 (Internal Server Error).
/// </summary>
public sealed class InternalError : Error
{
    public InternalError(string description)
        : base(StatusCodes.Status500InternalServerError, description) { }

    public InternalError()
        : base(StatusCodes.Status500InternalServerError, "Internal server error occurred!") { }
}