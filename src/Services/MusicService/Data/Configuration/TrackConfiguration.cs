using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.Property(t => t.Id).IsRequired();

        builder.Property(t => t.Title).IsRequired();

        builder.Property(t => t.Slug).IsRequired();

        builder.Property(t => t.ReleaseId).IsRequired();

        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.Slug).IsUnique();
    }
}