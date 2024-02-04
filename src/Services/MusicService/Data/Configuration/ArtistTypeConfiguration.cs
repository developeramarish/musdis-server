using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ArtistTypeConfiguration : IEntityTypeConfiguration<ArtistType>
{
    public void Configure(EntityTypeBuilder<ArtistType> builder)
    {
        builder.Property(at => at.Id).IsRequired();

        builder.Property(at => at.Name).IsRequired();

        builder.Property(at => at.Slug).IsRequired();

        builder.HasKey(at => at.Id);
        builder.HasIndex(at => at.Slug).IsUnique();
        builder.HasIndex(at => at.Name).IsUnique();
    }
}