using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.Property(a => a.Id).IsRequired();

        builder.Property(a => a.Name).IsRequired();

        builder.Property(a => a.Slug).IsRequired();

        builder.Property(a => a.CoverUrl).IsRequired();

        builder.Property(a => a.CreatorId).IsRequired();

        builder.Property(a => a.ArtistTypeId).IsRequired();
        builder.HasOne(a => a.ArtistType)
            .WithMany(at => at.Artists)
            .HasForeignKey(a => a.ArtistTypeId);

        builder.HasMany(a => a.ArtistUsers)
            .WithOne(au => au.Artist)
            .HasForeignKey(au => au.ArtistId);

        builder.HasMany(a => a.Tracks)
            .WithMany(t => t.Artists)
            .UsingEntity<TrackArtist>();

        builder.HasMany(a => a.Releases)
            .WithMany(r => r.Artists)
            .UsingEntity<ReleaseArtist>();

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Slug).IsUnique();
        builder.HasIndex(a => a.Name).IsUnique();
    }
}