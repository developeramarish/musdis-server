using Microsoft.AspNetCore.Authorization;

namespace Musdis.IdentityService.Authorization;

/// <summary>
///     The admin user authorization requirement.
/// </summary>
public sealed class AdminRequirement : IAuthorizationRequirement
{ }