using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an error indicating an internal server error,
///     associated with an HTTP status code 500.
/// </summary>
public sealed class InternalServerError : HttpError
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InternalServerError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    public InternalServerError(string description) : base(
        StatusCodes.Status500InternalServerError,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    /// <inheritdoc cref="InternalServerError.InternalServerError(string)"/>
    public InternalServerError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Internal server error occurred!";
}