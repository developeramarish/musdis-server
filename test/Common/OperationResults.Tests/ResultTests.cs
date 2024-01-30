using Musdis.OperationResults;

namespace  Musdis.OperationResults.Tests;

public sealed class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessResult_Always()
    {
        var result = Result.Success();

        var expectedIsSuccess = true;
        var expectedIsFailure = false;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult_WhenValidErrorPassed()
    {
        var error = new Error("some error");
        var result = Result.Failure(error);

        var expectedIsSuccess = false;
        var expectedIsFailure = true;
        var expectedError = error;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedIsFailure, result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ThrowsArgumentException_WhenNullErrorPassed()
    {
        var createFailure = () => Result.Failure((Error)null!);
        Assert.Throws<ArgumentException>(createFailure);
    }
}