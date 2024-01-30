using Musdis.OperationResults;

using Musdis.OperationResults.Tests.Models;

namespace Musdis.OperationResults.Tests;

public sealed class ValueResultTests
{
    [Fact]
    public void Success_ReturnsIntSuccessResult_WhenIntPassed()
    {
        var value = 420;
        var result = Result<int>.Success(value);

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Success_ReturnsNullIntSuccessResult_WhenNullIntPassed()
    {
        int? value = null;
        var result = Result<int?>.Success(value);

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Success_ReturnsClassSuccessResult_WhenClassPassed()
    {
        var value = new Person(69, "Pot");
        var result = Result<Person>.Success(value);

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Success_ReturnsNullClassSuccessResult_WhenNullClassPassed()
    {
        Person? value = null;
        var result = Result<Person>.Success(value!);

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ReturnsIntFailureResult_WhenValidErrorPassed()
    {
        var error = new Error("some error");
        var result = Result<int>.Failure(error);

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedValue = default(int);
        var expectedError = error;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ReturnsClassFailureResult_WhenValidErrorPassed()
    {
        var error = new Error("some error");
        var result = Result<Person>.Failure(error);

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedValue = default(Person);
        var expectedError = error;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ReturnsClassFailureResult_WhenValidErrorDescriptionPassed()
    {
        var result = Result<Person>.Failure("some error");

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedValue = default(Person);
        var expectedErrorDescription = "some error";

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedErrorDescription, result.Error!.Description);
    }

    [Fact]
    public void Failure_ThrowsArgumentException_WhenNullErrorPassed()
    {
        var createFailure = () => Result<int>.Failure((Error)null!);
        Assert.Throws<ArgumentException>(createFailure);
    }
}