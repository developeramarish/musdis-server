using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents a conflict with the current state of the target resource, 
///     associated with an HTTP status code 409.
/// </summary>
public sealed class ConflictError : HttpError
{
    public ConflictError(string description) : base(
        StatusCodes.Status409Conflict,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    public ConflictError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    
    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Conflict with the current state of the target resource.";
}