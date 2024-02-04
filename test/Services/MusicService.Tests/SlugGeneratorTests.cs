using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using MockQueryable.NSubstitute;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Services;

using NSubstitute;

using Slugify;

namespace Musdis.MusicService.Tests;


public sealed class SlugGeneratorTests
{

    [Theory]
    [InlineData("Hello world!", "hello-world")]
    [InlineData("   Hello world!   ", "hello-world")]
    [InlineData("  ", "")]
    public void Generate_ReturnsCorrectSuccessResult_WhenOneStringPassed(
        string value,
        string expected
    )
    {
        // Arrange
        var sut = new SlugGenerator(new SlugHelper(), null!); // no need of db context here

        // Act
        var generated = sut.Generate(value);

        // Assert
        Assert.True(generated.IsSuccess);
        Assert.False(generated.IsFailure);
        Assert.Equal(expected, generated.Value);
    }

    [Theory]
    [MemberData(nameof(MultipleStringData))]
    public void Generate_ReturnsCorrectSuccessResult_WhenMultipleStringPassed(
        string first,
        string[] rest,
        string expected
    )
    {
        // Arrange
        var sut = new SlugGenerator(new SlugHelper(), null!); // no need of db context here

        // Act
        var generated = sut.Generate(first, rest);

        // Assert
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
    public void Generate_ReturnsCorrectErrorResult_WhenNullStringsPassed()
    {
        // Arrange
        var sut = new SlugGenerator(new SlugHelper(), null!); // no need of db context here

        // Act
        var generated = sut.Generate("some string", null!);

        // Assert
        Assert.False(generated.IsSuccess);
        Assert.True(generated.IsFailure);
    }

    [Fact]
    public async Task GenerateUniqueSlugAsync_ReturnCorrect_WhenNotUniqueValuePassedAsync()
    {
        // Arrange
        var dbContext = Substitute.For<IMusicServiceDbContext>();

        string[] slugs = ["artist-one", "artist-one-1", "artist-two", "artist-three"];
        var slugsMock = slugs.BuildMock();

        dbContext
            .SqlQuery<string>(Arg.Any<FormattableString>())
            .ReturnsForAnyArgs(slugsMock);

        var sut = new SlugGenerator(new SlugHelper(), dbContext);

        // Act
        var slugResult = await sut.GenerateUniqueSlugAsync<Artist>("Artist One");

        // Assert
        Assert.Null(slugResult.Error);
        Assert.True(slugResult.IsSuccess);
        Assert.Equal("artist-one-2", slugResult.Value);
    }
}