using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using MockQueryable.NSubstitute;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Services;
using Musdis.MusicService.Tests.Fixtures;

using NSubstitute;

using Slugify;

namespace Musdis.MusicService.Tests;

// TODO change tests
public sealed class SlugGeneratorTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture;
    public SlugGeneratorTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

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
    [InlineData("hello world", "hello-world")]
    [InlineData("  hello world  ", "hello-world")]
    [InlineData("    t       ", "t")]
    [InlineData(" ", "")]
    [InlineData("           ", "")]
    [InlineData("\n", "")]
    [InlineData("\t", "")]
    public void Generate_ReturnsCorrectSuccessResult_WhenMultipleStringPassed(
        string first,
        string expected
    )
    {
        // Arrange
        var sut = new SlugGenerator(new SlugHelper(), null!); // no need of db context here

        // Act
        var generated = sut.Generate(first);

        // Assert
        Assert.True(generated.IsSuccess);
        Assert.False(generated.IsFailure);
        Assert.Equal(expected, generated.Value);
    }

    [Fact]
    public void Generate_ReturnsCorrectErrorResult_WhenNullStringsPassed()
    {
        // Arrange
        var sut = new SlugGenerator(new SlugHelper(), null!); // no need of db context here

        // Act
        var generated = sut.Generate(null!);

        // Assert
        Assert.False(generated.IsSuccess);
        Assert.True(generated.IsFailure);
    }

    [Fact]
    public async Task GenerateUniqueSlugAsync_ReturnCorrect_WhenNotUniqueValuePassedAsync()
    {
        // Arrange
        var artistType = await _databaseFixture.DbContext.ArtistTypes
            .AsNoTracking()
            .FirstAsync();

        _databaseFixture.DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist One",
            Slug = "artist-one",
            CoverUrl = "some-url",
            ArtistTypeId = artistType.Id,
            CreatorId = "someId"
        });
        _databaseFixture.DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist Two",
            Slug = "artist-two",
            CoverUrl = "some-url",
            ArtistTypeId = artistType.Id,
            CreatorId = "someId"
        });
        _databaseFixture.DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist one!",
            Slug = "artist-one-1",
            CoverUrl = "some-url",
            ArtistTypeId = artistType.Id,
            CreatorId = "someId"
        });

        _databaseFixture.DbContext.SaveChanges();

        var sut = new SlugGenerator(new SlugHelper(), _databaseFixture.DbContext);

        // Act
        var slugResult = await sut.GenerateUniqueSlugAsync<Artist>("Artist One");

        // Assert
        Assert.Null(slugResult.Error);
        Assert.True(slugResult.IsSuccess);
        Assert.Equal("artist-one-2", slugResult.Value);
    }
}