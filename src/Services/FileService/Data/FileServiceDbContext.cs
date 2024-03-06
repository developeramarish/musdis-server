using Microsoft.EntityFrameworkCore;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data;

/// <summary>
///     The database context for the file service.
/// </summary>
/// 
/// <param name="options">
///     The options for the database context.
/// </param>
public sealed class FileServiceDbContext(DbContextOptions<FileServiceDbContext> options)
    : DbContext(options)
{
    /// <summary>
    ///     The set of file infos.
    /// </summary>
    public DbSet<FileDetails> FileInfos => Set<FileDetails>();

    /// <summary>
    ///     The set of file types.
    /// </summary>
    public DbSet<FileType> FileTypes => Set<FileType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.Entity<FileType>().HasData(
            new()
            {
                Id = Guid.NewGuid(),
                Name = "image"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "audio"
            }
        );
    }
}