using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

using Musdis.OperationResults.Tests.Models;

namespace Musdis.OperationResults.Tests;

public sealed class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ReturnsSuccessResult_WhenIntValuePassed()
    {
        var value = 12;
        var result = value.ToValueResult();

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
        var result = value.ToValueResult();

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
        var result = value.ToValueResult();

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
        var result = value.ToValueResult();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        var expectedValue = value;
        Error? expectedError = null;

        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(expectedError, result.Error);
    }
}