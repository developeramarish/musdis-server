using Microsoft.AspNetCore.Authorization;

namespace Musdis.MusicService.Authorization.Requirements;

/// <summary>
///     The same author or admin user authorization requirement.
/// </summary>
public sealed class SameAuthorOrAdminRequirement : IAuthorizationRequirement
{ }