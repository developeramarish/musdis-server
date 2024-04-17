using Musdis.IdentityService.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Musdis.IdentityService.Data;

/// <summary>
///     The database context for the identity service.
/// </summary>
/// 
/// <param name="options">
///     The options for the database context.
/// </param>
public class IdentityServiceDbContext(
    DbContextOptions<IdentityServiceDbContext> options
) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>(x =>
        {
            x.Property(u => u.AvatarFileId).IsRequired(false);
            x.Property(u => u.AvatarUrl).IsRequired(false);
        });
    }
}
