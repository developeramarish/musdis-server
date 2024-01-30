using System.Diagnostics.CodeAnalysis;

using Musdis.OperationResults.Contracts;

namespace Musdis.OperationResults;

/// <inheritdoc cref="IResult"/>
public class Result : IResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    private Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null ||
            !isSuccess && error is null
        )
        {
            throw new ArgumentException("Invalid error object.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Result"/> class representing a successful result.
    /// </summary>
    /// 
    /// <returns>
    ///     A new <see cref="Result"/> instance indicating success with no associated error.
    /// </returns>
    public static Result Success()
    {
        return new(true, null);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Result"/> class 
    ///     representing a failure with the specified error.
    /// </summary>
    /// 
    /// <param name="error">
    ///     The error associated with the failure result.
    /// </param>
    /// 
    /// <returns>
    ///     A new <see cref="Result"/> instance indicating failure with the provided error.
    /// </returns>
    public static Result Failure(Error error)
    {
        return new(false, error);
    }

    /// <summary>
    ///     <inheritdoc cref="Failure(Error)" />
    /// </summary>
    /// 
    /// <param name="errorDescription">
    ///     The description of the error.
    /// </param>
    /// 
    /// <returns>
    ///     A new <see cref="Result"/> instance indicating failure with the provided error description.
    /// </returns>
    public static Result Failure(string errorDescription)
    {
        return new(false, new Error(errorDescription));
    }
}