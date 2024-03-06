using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

public sealed class FileTypeConfiguration : IEntityTypeConfiguration<FileType>
{
    public void Configure(EntityTypeBuilder<FileType> builder)
    {
        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.Name).IsRequired();

        builder.HasKey(x => x.Id);
    }
}