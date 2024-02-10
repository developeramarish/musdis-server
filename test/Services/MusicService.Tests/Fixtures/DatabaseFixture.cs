using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;

namespace Musdis.MusicService.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public MusicServiceDbContext DbContext { get; set; }
    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<MusicServiceDbContext>()
             .UseInMemoryDatabase("MusdisMusicDb")
             .Options;

        var dbContext = new MusicServiceDbContext(options);
        dbContext.Database.EnsureDeleted();

        SeedData();

        dbContext.Database.EnsureCreated();

        DbContext = dbContext;
    }


    private void SeedData()
    {
        if (DbContext is null)
        {
            return;
        }

        var artistTypeId = Guid.NewGuid();

        DbContext.ArtistTypes.Add(new ArtistType
        {
            Id = artistTypeId,
            Name = "Band",
            Slug = "band"
        });

        DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist One",
            Slug = "artist-one",
            CoverUrl = "some-url",
            ArtistTypeId = artistTypeId,
            CreatorId = "someId"
        });
        DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist Two",
            Slug = "artist-two",
            CoverUrl = "some-url",
            ArtistTypeId = artistTypeId,
            CreatorId = "someId"
        });
        DbContext.Artists.Add(new Artist
        {
            Id = Guid.NewGuid(),
            Name = "Artist one!",
            Slug = "artist-one-1",
            CoverUrl = "some-url",
            ArtistTypeId = artistTypeId,
            CreatorId = "someId"
        });

        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}