using Musdis.IdentityService.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Musdis.IdentityService.Defaults;

namespace Musdis.IdentityService.Data;
public class IdentityServiceDbContext : IdentityDbContext<User>
{
    public IdentityServiceDbContext(
        DbContextOptions<IdentityServiceDbContext> options
    ) : base(options)
    {
        Database.EnsureCreated();
    }
}
