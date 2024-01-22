using Musdis.MusicService.Services;

namespace Musdis.MusicService.Tests;


public class SlugGeneratorTests
{
    [Theory]
    [InlineData("Hello world!", "hello-world")]
    [InlineData("   Hello world!   ", "hello-world")]
    [InlineData("  ", "")]
    public void SlugGenerator_ReturnsCorrectSuccessResult_WhenOneStringPassed(
        string value,
        string expected
    ) 
    {
        var sut = new SlugGenerator();

        var generated = sut.Generate(value);
        
        Assert.True(generated.IsSuccess);
        Assert.False(generated.IsFailure);
        Assert.Equal(expected, generated.Value);
    }

    [Fact]
    public void SlugGenerator_ReturnsCorrectErrorResult_WhenNullStringsPassed() 
    {
        var sut = new SlugGenerator();

        var generated = sut.Generate("some string", null!);
        
        Assert.False(generated.IsSuccess);
        Assert.True(generated.IsFailure);
    }
}