using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an error indicating an internal server error,
///     associated with an HTTP status code 500.
/// </summary>
public sealed class InternalServerError : HttpError
{
    public InternalServerError(string description) : base(
        StatusCodes.Status500InternalServerError,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    public InternalServerError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Internal server error occurred!";
}