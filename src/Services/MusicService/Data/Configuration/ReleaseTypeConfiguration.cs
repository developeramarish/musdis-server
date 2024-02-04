using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ReleaseTypeConfiguration : IEntityTypeConfiguration<ReleaseType>
{
    public void Configure(EntityTypeBuilder<ReleaseType> builder)
    {
        builder.Property(rt => rt.Id).IsRequired();

        builder.Property(rt => rt.Name).IsRequired();

        builder.Property(rt => rt.Slug).IsRequired();

        builder.HasKey(rt => rt.Id);
        builder.HasIndex(rt => rt.Slug).IsUnique();
        builder.HasIndex(rt => rt.Name).IsUnique();
    }
}