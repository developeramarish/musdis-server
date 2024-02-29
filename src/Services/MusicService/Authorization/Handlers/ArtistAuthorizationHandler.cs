using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using Musdis.MusicService.Authorization.Requirements;
using Musdis.MusicService.Models;

namespace Musdis.MusicService.Authorization.Handlers;

public sealed class ArtistAuthorizationHandler
    : AuthorizationHandler<SameAuthorOrAdminRequirement, Artist>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameAuthorOrAdminRequirement requirement,
        Artist resource
    )
    {
        ArgumentNullException.ThrowIfNull(resource);

        var userId = context.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)
            ?.Value;

        if (userId == resource.CreatorId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}