using Microsoft.IdentityModel.Tokens;

using Musdis.IdentityService.Errors;
using Musdis.OperationResults;

namespace Musdis.IdentityService.Extensions;

/// <summary>
/// Extensions for <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Converts <see cref="Error"/> object to problem details result.
    /// </summary>
    /// <param name="error">Error to convert.</param>
    /// <param name="instance">URL instance of occurred error.</param>
    /// <returns>Problem details result.</returns>
    public static IResult ToProblemResult(this Error error, string? instance = null)
    {
        (string Title, Dictionary<string, object?>? Extensions) details = error switch
        {
            InternalError => ("Internal error!", null),
            ValidationError validationError when !validationError.Failures.IsNullOrEmpty() =>
                ("Validation error!", new() { { "failures", validationError.Failures } }),
            ValidationError validationError when validationError.Failures.IsNullOrEmpty() =>
                ("Validation error!", null),

            _ => ("Internal error!", null),
        };

        return Results.Problem(
            title: details.Title,
            statusCode: error.Code,
            detail: error.Description,
            extensions: details.Extensions,
            instance: instance
        );
    }
}

