using Musdis.MusicService.Services;

using Slugify;

namespace Musdis.MusicService.Tests;


public sealed class SlugGeneratorTests
{
    private readonly SlugGenerator _sut = new(new SlugHelper());

    [Theory]
    [InlineData("Hello world!", "hello-world")]
    [InlineData("   Hello world!   ", "hello-world")]
    [InlineData("  ", "")]
    public void SlugGenerator_ReturnsCorrectSuccessResult_WhenOneStringPassed(
        string value,
        string expected
    )
    {
        var generated = _sut.Generate(value);

        Assert.True(generated.IsSuccess);
        Assert.False(generated.IsFailure);
        Assert.Equal(expected, generated.Value);
    }

    [Theory]
    [MemberData(nameof(MultipleStringData))]
    public void SlugGenerator_ReturnsCorrectSuccessResult_WhenMultipleStringPassed(
        string first,
        string[] rest,
        string expected
    )
    {
        var generated = _sut.Generate(first, rest);

        Assert.True(generated.IsSuccess);
        Assert.False(generated.IsFailure);
        Assert.Equal(expected, generated.Value);
    }

    public static IEnumerable<object[]> MultipleStringData()
    {
        return [
            ["hello", new string[] { "world" }, "hello-world"],
            ["  ", new string[] { "hello ", "world!" }, "hello-world"],
            ["+", new string[] { " ", "hello   ", "world!", "  " }, "-hello-world"],
            ["+hello", new string[] { " ", "hello   ", "world!", "  " }, "hello-hello-world"],
        ];
    }

    [Fact]
    public void SlugGenerator_ReturnsCorrectErrorResult_WhenNullStringsPassed()
    {
        var generated = _sut.Generate("some string", null!);

        Assert.False(generated.IsSuccess);
        Assert.True(generated.IsFailure);
    }
}