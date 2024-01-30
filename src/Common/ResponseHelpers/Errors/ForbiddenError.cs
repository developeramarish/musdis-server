using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an error indicating  that the server understood the request 
///     but refuses to authorize it, associated with an HTTP status code 403.
/// </summary>
public sealed class ForbiddenError : HttpError
{

    public ForbiddenError(string description) : base(
        StatusCodes.Status403Forbidden,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    public ForbiddenError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Access to the requested resource is forbidden.";
}