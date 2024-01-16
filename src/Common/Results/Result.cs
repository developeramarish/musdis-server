using System.Diagnostics.CodeAnalysis;

using Results.Contracts;

namespace Results;

/// <inheritdoc cref="IResult"/>
public class Result : IResult
{
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
    /// Creates a new instance of the <see cref="Result"/> class representing a successful result.
    /// </summary>
    /// <returns>A new <see cref="Result"/> instance indicating success with no associated error.</returns>
    public static Result Success()
    {
        return new(true, null);
    }
    /// <summary>
    /// Creates a new instance of the <see cref="Result"/> class representing a failure with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure result.</param>
    /// <returns>A new <see cref="Result"/> instance indicating failure with the provided error.</returns>
    public static Result Failure(Error error)
    {
        return new(false, error);
    }
}

/// <inheritdoc cref="IValueResult{TValue}"/>
public class Result<TValue> : IValueResult<TValue>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }
    public TValue? Value { get; private set; }
    private Result(TValue? value, bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null ||
            !isSuccess && error is null ||
            !EqualityComparer<TValue>.Default.Equals(default, value) &&
            error is not null
        )
        {
            throw new ArgumentException($"Error cannot be not null if value is not default");
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TValue}"/> class representing a successful result with the specified value.
    /// </summary>
    /// <param name="value">The associated value for the successful result.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance indicating success with the provided value.</returns>
    public static Result<TValue> Success(TValue value)
    {
        return new(value, true, null);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TValue}"/> class representing a failure with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure result.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance indicating failure with the provided error.</returns>
    public static Result<TValue> Failure(Error error)
    {
        return new(default, false, error);
    }
}
