using System.Text;

using FluentValidation.Results;

using IdentityService.Errors;

namespace IdentityService.Extensions;

/// <summary>
/// Provides extension methods for working with FluentValidation library results.
/// </summary>
public static class ValidationFailuresExtensions
{
    /// <summary>
    /// Converts a collection of validation failures into a <see cref="ValidationError"/> containing error details.
    /// </summary>
    /// <param name="failures">The collection of validation failures to be converted.</param>
    /// <returns>A <see cref="ValidationError"/> containing the details of the validation failures.</returns>
    public static ValidationError ToValidationError(
        this IEnumerable<ValidationFailure> failures
    )
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Validation error occured. Details: ");
        foreach (var failure in failures)
        {
            stringBuilder.Append(failure.ErrorMessage);
            stringBuilder.AppendLine();
        }

        return new ValidationError(stringBuilder.ToString());
    }
}