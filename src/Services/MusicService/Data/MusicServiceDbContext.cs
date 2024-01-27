using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data;

/// <summary>
/// Application database context.
/// </summary>
/// <param name="options">Options to configure EF Core.</param>
public sealed class MusicServiceDbContext(
    DbContextOptions<MusicServiceDbContext> options
) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<ArtistUser> ArtistUsers => Set<ArtistUser>();
    public DbSet<ArtistType> ArtistTypes => Set<ArtistType>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<ReleaseArtist> ReleaseArtists => Set<ReleaseArtist>();
    public DbSet<ReleaseType> ReleaseTypes => Set<ReleaseType>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<TagTrack> TagTracks => Set<TagTrack>();
}
