using Microsoft.AspNetCore.Authorization;

namespace Musdis.MusicService.Authorization.Requirements;

/// <summary>
///     The admin user authorization requirement.
/// </summary>
public sealed class AdminRequirement : IAuthorizationRequirement
{ }