using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data;

/// <summary>
///     Application database context.
/// </summary>
public interface IMusicServiceDbContext : IDisposable
{      
    /// <summary>
    ///     Creates a LINQ query based on a raw SQL query, which returns a result set of 
    ///     a scalar type natively supported by the database provider.
    /// </summary>
    /// 
    /// <typeparam name="T"></typeparam>
    /// <param name="query">
    ///     The interpolated string representing a SQL query with parameters.
    /// </param>
    /// 
    /// <returns>
    ///     An <see cref="IQueryable{T}"/> representing the interpolated string SQL query.
    /// </returns>
    IQueryable<T> SqlQuery<T>(FormattableString query);

    /// <inheritdoc cref="DbContext.SaveChanges()"/>
    int SaveChanges();

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public DbSet<Artist> Artists { get; }
    public DbSet<ArtistUser> ArtistUsers { get; }
    public DbSet<ArtistType> ArtistTypes { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<Release> Releases { get; }
    public DbSet<ReleaseArtist> ReleaseArtists { get; }
    public DbSet<ReleaseType> ReleaseTypes { get; }
    public DbSet<Track> Tracks { get; }
    public DbSet<TagTrack> TagTracks { get; }
    public DbSet<TrackArtist> TrackArtists { get; }
}