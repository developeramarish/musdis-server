using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an unauthorized error, associated with an HTTP status code 401.
/// </summary>
public sealed class UnauthorizedError : HttpError
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UnauthorizedError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    public UnauthorizedError(string description) : base(
        StatusCodes.Status401Unauthorized,
        description,
        ProblemDetailsType,
        ErrorTitle
    ) { }

    /// <inheritdoc cref="UnauthorizedError.UnauthorizedError(string)"/>
    public UnauthorizedError() : this("User is not authorized") { }
    
        
    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Unauthorized.";
}