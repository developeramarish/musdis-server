using Musdis.OperationResults;

namespace Musdis.IdentityService.Errors;

/// <summary>
/// Represents an unauthorized error, associated with an HTTP status code 401.
/// </summary>
public sealed class UnauthorizedError : Error
{
    public UnauthorizedError(string description)
        : base(401, description) { }

    public UnauthorizedError()
        : base(401, "User is not authorized") { }
}