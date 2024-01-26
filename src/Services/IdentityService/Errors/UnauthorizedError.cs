using Musdis.OperationResults;

namespace Musdis.IdentityService.Errors;

/// <summary>
/// Represents an unauthorized error, associated with an HTTP status code 401.
/// </summary>
public sealed class UnauthorizedError : Error
{
    public UnauthorizedError(string description)
        : base(StatusCodes.Status401Unauthorized, description) { }

    public UnauthorizedError()
        : base(StatusCodes.Status401Unauthorized, "User is not authorized") { }
}