using Microsoft.AspNetCore.Authorization;

using Musdis.MusicService.Authorization.Requirements;

namespace Musdis.MusicService.Authorization;

public sealed class AdminAuthorizationHandler : AuthorizationHandler<SameAuthorOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SameAuthorOrAdminRequirement requirement
    )
    {
        throw new NotImplementedException();
    }
}