using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Musdis.FileService.Models;

namespace Musdis.FileService.Data.Configuration;

public sealed class AvatarFileConfiguration : IEntityTypeConfiguration<AvatarFile>
{
    public void Configure(EntityTypeBuilder<AvatarFile> builder)
    {
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.FileDetailsId).IsRequired();
        builder
            .HasOne(x => x.FileDetails)
            .WithMany()
            .HasForeignKey(x => x.FileDetailsId);

        builder.HasKey(x => new { x.UserId, x.FileDetailsId });
    }
}