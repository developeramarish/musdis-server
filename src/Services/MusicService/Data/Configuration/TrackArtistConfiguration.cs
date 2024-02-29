using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class TrackArtistConfiguration : IEntityTypeConfiguration<TrackArtist>
{
    public void Configure(EntityTypeBuilder<TrackArtist> builder)
    {
        builder.Property(ra => ra.ArtistId).IsRequired();

        builder.Property(ra => ra.TrackId).IsRequired();

        builder.HasKey(ra => new { ra.TrackId, ra.ArtistId });
    }
}