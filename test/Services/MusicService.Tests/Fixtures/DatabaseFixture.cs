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

        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}