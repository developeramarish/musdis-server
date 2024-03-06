using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

public sealed class TrackFileConfiguration : IEntityTypeConfiguration<TrackFile>
{
    public void Configure(EntityTypeBuilder<TrackFile> builder)
    {
        builder.Property(x => x.TrackId).IsRequired();

        builder.Property(x => x.FileDetailsId).IsRequired();
        builder
            .HasOne(x => x.FileDetails)
            .WithMany()
            .HasForeignKey(x => x.FileDetailsId);

        builder.HasKey(x => new { x.TrackId, x.FileDetailsId });
    }
}