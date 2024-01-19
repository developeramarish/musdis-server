using Musdis.IdentityService.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musdis.IdentityService.Models.Entities;

namespace Musdis.IdentityService.Data;
public class IdentityServiceDbContext(
    DbContextOptions<IdentityServiceDbContext> options
) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
