using Results;

namespace IdentityService.Errors;

/// <summary>
/// Represents an error indicating an internal server error, 
/// associated with HTTP status code 500 (Internal Server Error).
/// </summary>
public sealed class InternalError : Error
{
    public InternalError(string description)
        : base(500, description) { }

    public InternalError()
        : base(500, "Internal error!") { }
}