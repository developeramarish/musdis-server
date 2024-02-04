using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.IdentityService.Data;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.IdentityService.Endpoints;

// TODO add paged response
public static class UserEndpoints
{
    public static RouteGroupBuilder MapUsers(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", async (
            CancellationToken cancellationToken,
            [FromServices] IdentityServiceDbContext dbContext,
            [FromQuery] int limit = 0,
            [FromQuery] int offset = 0
        ) =>
        {
            var users = await dbContext.Users
                .AsNoTracking()
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return Results.Ok(users);
        });

        groupBuilder.MapGet("/{idOrUsername:string}", async (
            [FromRoute] string idOrUserName,
            [FromServices] IdentityServiceDbContext dbContext,
            CancellationToken cancellationToken,
            HttpContext context
        ) =>
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == idOrUserName, cancellationToken);
            if (user is not null)
            {
                return Results.Ok(user);
            }

            user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == idOrUserName, cancellationToken);
            if (user is not null)
            {
                return Results.Ok(user);
            }

            return new NotFoundError(
                $"User with Id or UserName = {idOrUserName}"
            ).ToProblemHttpResult(context.Request.Path);
        });

        return groupBuilder;
    }
}