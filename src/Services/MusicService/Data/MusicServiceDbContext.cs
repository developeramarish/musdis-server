using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data;

/// <summary>
///     Application database context.
/// </summary>
public sealed class MusicServiceDbContext : DbContext, IMusicServiceDbContext
{
    public MusicServiceDbContext(DbContextOptions<MusicServiceDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<ArtistUser> ArtistUsers => Set<ArtistUser>();
    public DbSet<ArtistType> ArtistTypes => Set<ArtistType>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<ReleaseArtist> ReleaseArtists => Set<ReleaseArtist>();
    public DbSet<ReleaseType> ReleaseTypes => Set<ReleaseType>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<TagTrack> TagTracks => Set<TagTrack>();
    public DbSet<TrackArtist> TrackArtists => Set<TrackArtist>();

    public IQueryable<T> SqlQuery<T>(FormattableString query)
    {
        return Database.SqlQuery<T>(query);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        if (!ArtistTypes.Any())
        {
            DataSeeder.SeedArtistTypes(modelBuilder);
        }
        if (!ReleaseTypes.Any())
        {
            DataSeeder.SeedReleaseTypes(modelBuilder);
        }
    }
}
