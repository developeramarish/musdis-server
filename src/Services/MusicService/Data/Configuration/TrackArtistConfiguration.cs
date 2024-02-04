using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class TrackArtistConfiguration : IEntityTypeConfiguration<TrackArtist>
{
    public void Configure(EntityTypeBuilder<TrackArtist> builder)
    {
        builder.Property(ra => ra.ArtistId).IsRequired();
        // builder.HasOne(ra => ra.Artist)
        //     .WithMany()
        //     .HasForeignKey(ra => ra.ArtistId);

        builder.Property(ra => ra.TrackId).IsRequired();
        // builder.HasOne(ra => ra.Track)
        //     .WithMany()
        //     .HasForeignKey(ra => ra.TrackId);

        builder.HasKey(ra => new { ra.TrackId, ra.ArtistId });
    }
}