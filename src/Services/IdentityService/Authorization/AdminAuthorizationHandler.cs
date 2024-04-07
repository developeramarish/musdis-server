using Microsoft.AspNetCore.Authorization;

using Musdis.AuthHelpers.Authorization;

namespace Musdis.IdentityService.Authorization;

public sealed class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminRequirement requirement
    )
    {
        var adminClaim = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimDefaults.Admin)?.Value;

        if (adminClaim == "true")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
