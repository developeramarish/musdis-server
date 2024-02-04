using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ReleaseArtistConfiguration : IEntityTypeConfiguration<ReleaseArtist>
{
    public void Configure(EntityTypeBuilder<ReleaseArtist> builder)
    {
        builder.Property(ra => ra.ArtistId).IsRequired();

        builder.Property(ra => ra.ReleaseId).IsRequired();

        builder.HasKey(ra => new { ra.ReleaseId, ra.ArtistId });
    }
}