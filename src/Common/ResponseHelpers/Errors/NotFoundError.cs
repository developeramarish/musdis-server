using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents a resource not found error, associated with an HTTP status code 404.
/// </summary>
public sealed class NotFoundError : HttpError
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    public NotFoundError(string description) : base(
        StatusCodes.Status404NotFound,
        description,
        ProblemDetailsType,
        ErrorTitle
    )
    { }

    /// <inheritdoc cref="NotFoundError.NotFoundError(string)"/>
    public NotFoundError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Requested resource is not found.";
}