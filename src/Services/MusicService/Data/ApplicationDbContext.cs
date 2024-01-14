using Microsoft.EntityFrameworkCore;

using MusicService.Models;

namespace MusicService.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<ArtistUser> ArtistUsers => Set<ArtistUser>();
    public DbSet<ArtistType> ArtistTypes => Set<ArtistType>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<ReleaseArtist> ReleaseArtists => Set<ReleaseArtist>();
    public DbSet<ReleaseType> ReleaseTypes => Set<ReleaseType>();
    public DbSet<Track> Tracks => Set<Track>();
}
