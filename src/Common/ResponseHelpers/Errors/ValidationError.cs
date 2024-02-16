using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents an error indicating a validation failure, associated with an HTTP status code 400.
/// </summary>
public sealed class ValidationError : HttpError
{
    /// <inheritdoc cref="HttpError.ErrorType"/>
    public static readonly string ProblemDetailsType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    /// <inheritdoc cref="HttpError.Title"/>
    public static readonly string ErrorTitle = "Bad request.";

    /// <summary>
    ///     Messages of failures that caused the error.
    /// </summary>
    public IEnumerable<string> FailureMessages { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    public ValidationError(string description) : base(
        StatusCodes.Status400BadRequest,
        description,
        ProblemDetailsType,
        ErrorTitle
    )
    {
        FailureMessages = [];
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    /// <param name="failureMessages">
    ///     A collection of failure messages. This collection will be added to Problem Details extension.
    /// </param>
    public ValidationError(
        string description,
        IEnumerable<string> failureMessages
    ) : this(description)
    {
        FailureMessages = failureMessages;
    }

    /// <inheritdoc cref="ValidationError.ValidationError(string)"/>
    public ValidationError() : this(ErrorTitle) { }

    /// <inheritdoc cref="HttpError.ToProblemHttpResult(string)"/>
    public override IResult ToProblemHttpResult(string instance)
    {
        return Results.Problem(
            type: ErrorType,
            statusCode: StatusCode,
            detail: Description,
            title: Title,
            instance: instance,
            extensions: FailureMessages.Any()
                ? new Dictionary<string, object?>
                {
                    { "failures", FailureMessages }
                }
                : null
        );
    }
}