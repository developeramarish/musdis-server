using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents a content not found error, associated with an HTTP status code 404.
/// </summary>
public sealed class NotFoundError : HttpError
{
    public NotFoundError(string description) : base(
        StatusCodes.Status404NotFound,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    public NotFoundError() : this(ErrorTitle) { }
        
    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    
    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Requested resource is not found.";
}