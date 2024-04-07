using Microsoft.AspNetCore.Authorization;

using Musdis.AuthHelpers.Authorization;
using Musdis.MusicService.Authorization.Requirements;

namespace Musdis.MusicService.Authorization;

public sealed class AdminAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var adminClaim = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimDefaults.Admin)?.Value;

        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (var requirement in pendingRequirements)
        {
            if (requirement is SameAuthorOrAdminRequirement or AdminRequirement &&
                adminClaim == "true"
            )
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}