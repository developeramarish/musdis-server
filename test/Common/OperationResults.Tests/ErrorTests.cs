using Musdis.OperationResults.Tests.Models;

namespace Musdis.OperationResults.Tests;

public sealed class ErrorTests
{
    [Fact]
    public void ToResult_ReturnsFailureResult_WhenErrorPassed()
    {
        var error = new Error("some error");
        var result = error.ToResult();

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedError = error;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsIntFailureResult_WhenErrorPassed()
    {
        var error = new Error("some error");
        var result = error.ToValueResult<int>();

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedValue = default(int);
        var expectedError = error;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsClassFailureResult_WhenErrorPassed()
    {
        var error = new Error("some error");
        var result = error.ToValueResult<Person>();

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedValue = default(Person);
        var expectedError = error;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }
}