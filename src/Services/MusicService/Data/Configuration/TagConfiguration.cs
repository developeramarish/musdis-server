using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Id).IsRequired();

        builder.Property(t => t.Name).IsRequired();

        builder.Property(t => t.Slug).IsRequired();

        builder.HasMany(t => t.Tracks)
            .WithMany(tr => tr.Tags)
            .UsingEntity<TagTrack>();

        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.HasIndex(t => t.Name).IsUnique();
    }
}