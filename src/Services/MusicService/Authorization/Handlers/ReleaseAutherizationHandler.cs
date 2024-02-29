using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Authorization.Requirements;
using Musdis.MusicService.Data;
using Musdis.MusicService.Models;

namespace Musdis.MusicService.Authorization;

public sealed class ReleaseAutherizationHandler 
    : AuthorizationHandler<SameAuthorOrAdminRequirement, Release>
{

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SameAuthorOrAdminRequirement requirement, 
        Release resource
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