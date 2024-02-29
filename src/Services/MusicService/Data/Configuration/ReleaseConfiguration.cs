using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ReleaseConfiguration : IEntityTypeConfiguration<Release>
{
    public void Configure(EntityTypeBuilder<Release> builder)
    {
        builder.Property(r => r.Id).IsRequired();

        builder.Property(r => r.ReleaseTypeId).IsRequired();

        builder.Property(r => r.Name).IsRequired();

        builder.Property(r => r.Slug).IsRequired();

        builder.Property(r => r.ReleaseDate).IsRequired();

        builder.Property(r => r.CoverUrl).IsRequired();

        builder.Property(r => r.CreatorId).IsRequired();

        builder.HasMany(r => r.Tracks)
            .WithOne(t => t.Release)
            .HasForeignKey(t => t.ReleaseId);

        builder.HasOne(r => r.ReleaseType)
            .WithMany(rt => rt.Releases)
            .HasForeignKey(r => r.ReleaseTypeId);

        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.Slug).IsUnique();
    }
}