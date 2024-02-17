using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.MusicService.Models;

namespace Musdis.MusicService.Data.Configuration;

public sealed class ArtistUserConfiguration : IEntityTypeConfiguration<ArtistUser>
{
    public void Configure(EntityTypeBuilder<ArtistUser> builder)
    {
        builder.Property(au => au.UserId).IsRequired();

        builder.Property(au => au.ArtistId).IsRequired();

        builder.Property(au => au.UserName).IsRequired();

        builder.HasKey(au => new { au.ArtistId, au.UserId });
    }
}