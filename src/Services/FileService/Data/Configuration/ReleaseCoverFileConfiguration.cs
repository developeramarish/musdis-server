using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

public sealed class ReleaseCoverFileConfiguration : IEntityTypeConfiguration<ReleaseCoverFile>
{
    public void Configure(EntityTypeBuilder<ReleaseCoverFile> builder)
    {
        builder.Property(x => x.ReleaseId).IsRequired();

        builder.Property(x => x.FileDetailsId).IsRequired();
        builder
            .HasOne(x => x.FileDetails)
            .WithMany()
            .HasForeignKey(x => x.FileDetailsId);

        builder.HasKey(x => new { x.ReleaseId, x.FileDetailsId });
    }
}