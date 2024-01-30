using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an error indicating that access to the target resource is no longer available, 
///     associated with an HTTP status code 410.
/// </summary>
public sealed class GoneError : HttpError
{

    public GoneError(string description) : base(
        StatusCodes.Status410Gone,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }
    public GoneError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Access to the target resource is no longer available!";

}