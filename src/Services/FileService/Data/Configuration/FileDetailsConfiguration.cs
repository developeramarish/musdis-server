using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

/// <summary>
///     The configuration for <see cref="FileDetails"/> model.
/// </summary>
public sealed class FileDetailsConfiguration : IEntityTypeConfiguration<FileDetails>
{
    public void Configure(EntityTypeBuilder<FileDetails> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        
        builder.Property(x => x.Name).IsRequired();
        
        builder.Property(x => x.OwnerId).IsRequired();

        builder.Property(x => x.FileTypeId).IsRequired();
        builder.HasOne(x => x.FileType)
            .WithMany()
            .HasForeignKey(x => x.FileTypeId);

        builder.HasKey(x => x.Id);
    }
}