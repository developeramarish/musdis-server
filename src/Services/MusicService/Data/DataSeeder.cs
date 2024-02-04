using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data;

/// <summary>
///     The helper class for seeding data into database.
/// </summary>
internal static class DataSeeder
{
    /// <summary>
    ///     Seeds <see cref="Tag"/> data into database.
    /// </summary>
    /// 
    /// <param name="modelBuilder">
    ///     The builder being used to seed the data.
    /// </param>
    public static void SeedTags(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>().HasData([
            new() {
                Id = Guid.NewGuid(),
                Name = "Lo-Fi",
                Slug = "lo-fi",
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Shoegaze",
                Slug = "shoegaze",
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Rock",
                Slug = "rock",
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Pop",
                Slug = "pop",
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Electronic",
                Slug = "Electronic",
            },
        ]);
    }

    /// <summary>
    ///     Seeds <see cref="ArtistType"/> data into database.
    /// </summary>
    /// 
    /// <param name="modelBuilder">
    ///     The builder being used to seed the data.
    /// </param>
    public static void SeedArtistTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArtistType>().HasData([
            new() {
                Id = Guid.NewGuid(),
                Name = "Band",
                Slug = "band",
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Musician",
                Slug = "musician",
            },
        ]);
    }

    /// <summary>
    ///     Seeds <see cref="ReleaseType"/> data into database.
    /// </summary>
    /// 
    /// <param name="modelBuilder">
    ///     The builder being used to seed the data.
    /// </param>
    public static void SeedReleaseTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReleaseType>().HasData([
            new(){
                Id = Guid.NewGuid(),
                Name = "EP",
                Slug = "ep"
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Album",
                Slug = "album"
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Single",
                Slug = "single"
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Soundtrack",
                Slug = "soundtrack"
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Other",
                Slug = "other"
            }
        ]);
    }
}