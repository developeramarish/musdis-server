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
    public DbSet<FileMetadata> FilesMetadata => Set<FileMetadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}