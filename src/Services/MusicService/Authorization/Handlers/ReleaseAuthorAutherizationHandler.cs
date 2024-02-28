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
    private readonly MusicServiceDbContext _dbContext;

    public ReleaseAutherizationHandler(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SameAuthorOrAdminRequirement requirement, 
        Release resource
    )
    {
        var artistIds = await _dbContext.ReleaseArtists
            .Where(ra => ra.ReleaseId == resource.Id)
            .Select(ra => ra.ArtistId)
            .ToArrayAsync();
        var userIds = await _dbContext.ArtistUsers
            .Where(au => artistIds.Contains(au.ArtistId))
            .Select(au => au.UserId)
            .ToArrayAsync();

        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIds.Contains(userId))
        {
            context.Succeed(requirement);
        }

        throw new NotImplementedException();
    }
}