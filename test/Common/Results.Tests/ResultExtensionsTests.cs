using Results.Extensions;
using Results.Tests.Models;

namespace Results.Tests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ReturnsSuccessResult_WhenIntValuePassed()
    {
        var value = 12;
        var result = value.ToResult();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsSuccessResult_WhenNullIntValuePassed()
    {
        int? value = null;
        var result = value.ToResult();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsSuccessResult_WhenClassValuePassed()
    {
        var value = new Person(21, "Hella");
        var result = value.ToResult();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsSuccessResult_WhenNullClassValuePassed()
    {
        Person? value = null;
        var result = value.ToResult();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void ToResult_ReturnsFailureResult_WhenErrorPassed()
    {
        var error = new Error(0, "some error");
        var result = error.ToResult<Error>();

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
        var error = new Error(0, "some error");
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
        var error = new Error(0, "some error");
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