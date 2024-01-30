using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an unauthorized error, associated with an HTTP status code 401.
/// </summary>
public sealed class UnauthorizedError : HttpError
{
    public UnauthorizedError(string description) : base(
        StatusCodes.Status401Unauthorized,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    public UnauthorizedError() : this("User is not authorized") { }
    
        
    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Unauthorized.";
}