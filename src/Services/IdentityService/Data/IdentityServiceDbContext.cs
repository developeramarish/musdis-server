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
) : IdentityDbContext<User>(options) { }
