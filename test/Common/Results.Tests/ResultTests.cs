namespace Results.Tests;

public class ResulTests
{
    [Fact]
    public void Success_ReturnsSuccessResult_Always()
    {
        var result = Result.Success();

        var expectedIsSuccess = true;
        var expextedIsFailure = false;
        Error? expectedError = null;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expextedIsFailure, result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult_WhenValidErrorPassed()
    {
        var error = new Error(0, "some error");
        var result = Result.Failure(error);

        var expectedIsSuccess = false;
        var expextedIsFailure = true;
        var expectedError = error;

        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expextedIsFailure, result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Failure_ThrowsArgumentException_WhenNullErrorPassed()
    {
        var createFailure = () => Result.Failure(null!);
        Assert.Throws<ArgumentException>(createFailure);
    }
}