using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents contract for HTTP errors, that could be converted 
///     into <see cref="ProblemDetailsInfo"/> object.
/// </summary>
/// <remarks>
///    More info in <seealso cref="https://datatracker.ietf.org/doc/html/rfc7807"/>
/// </remarks>
public class HttpError : Error
{
    protected HttpError(
        int code,
        string description,
        string errorType,
        string title
    ) : base(description)
    {
        StatusCode = code;
        ErrorType = errorType;
        Title = title;
    }

    /// <summary>
    ///     The type of an error in problem details specification.
    /// </summary>
    public string ErrorType { get; private set; }

    /// <summary>
    ///     A short, human-readable summary of the problem type.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    ///     The HTTP status code of the error.
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    ///     Converts error into problem details <see cref="IResult"/>.
    /// </summary>
    /// 
    /// <param name="instance">
    ///     A URI reference that identifies the specific occurrence of the problem.
    /// </param>
    /// 
    /// <returns>
    ///     <see cref="IResult"/> as problem details.
    /// </returns>
    public virtual IResult ToProblemHttpResult(string instance)
    {
        return Results.Problem(
            type: ErrorType,
            statusCode: StatusCode,
            detail: Description,
            title: Title,
            instance: instance
        );
    }
}