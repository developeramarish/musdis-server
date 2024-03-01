using FluentValidation.Results;

using Musdis.MusicService.Validation;
using Musdis.OperationResults;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Extensions;

/// <summary>
///     Extension methods for <see cref="IEnumerable{T}"/> of <see cref="ValidationFailure"/>.
/// </summary>
public static class ValidationFailuresExtensions
{
    /// <summary>
    ///     Maps an <see cref="IEnumerable{T}"/> of <see cref="ValidationFailure"/> to an <see cref="Error"/>
    /// </summary>
    /// 
    /// <param name="failures">
    ///     The <see cref="IEnumerable{T}"/> of <see cref="ValidationFailure"/> to map.
    /// </param>
    /// 
    /// <returns>
    ///     The mapped <see cref="Error"/>.
    /// </returns>
    public static Error ToError(
        this IEnumerable<ValidationFailure> failures,
        string message = "Validation error occurred."        
    )
    {
        if (failures.Any(e => e.ErrorCode == ErrorCodes.NonUniqueData))
        {
            var errorMessages = failures
                .Where(f => f.ErrorCode == ErrorCodes.NonUniqueData)
                .Select(f => f.ErrorMessage);
            var joined = string.Join("\n", errorMessages);

            return new ConflictError($"Some data is not unique: {joined}");
        }

        return new ValidationError(
            message,
            failures.Select(f => f.ErrorMessage)
        );
    }
}