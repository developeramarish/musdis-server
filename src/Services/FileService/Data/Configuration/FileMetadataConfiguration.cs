using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

/// <summary>
///     The configuration for <see cref="FileMetadata"/> model.
/// </summary>
public sealed class FileMetadataConfiguration : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder.ToTable("FilesMetadata");

        builder.Property(x => x.Id).IsRequired();
        
        builder.Property(x => x.Path).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        
        builder.Property(x => x.FileType).IsRequired();

        builder.HasKey(x => x.Id);
    }
}