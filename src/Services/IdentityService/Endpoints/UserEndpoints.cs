using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.IdentityService.Data;
using Musdis.IdentityService.Dtos;
using Musdis.IdentityService.Models;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

namespace Musdis.IdentityService.Endpoints;

/// <summary>
///     User related endpoints.
/// </summary>
public static class UserEndpoints
{
    public static RouteGroupBuilder MapUsers(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", async (
            [FromServices] IdentityServiceDbContext dbContext,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromQuery] int limit = 0,
            [FromQuery] int page = 1
        ) =>
        {
            var queryable = dbContext.Users.AsNoTracking();
            var totalCount = await queryable.CountAsync(cancellationToken);

            if (limit > 0)
            {
                var offset = (page - 1) * limit;
                queryable = queryable
                    .Skip(offset)
                    .Take(limit);
            }

            var users = await queryable.ToListAsync(cancellationToken);

            var dataResult = UserReadDto.FromUsers(users);
            if (dataResult.IsFailure)
            {
                return dataResult.Error.ToHttpResult(context.Request.Path);
            }

            var pagedResponse = new PagedDataResponse<UserReadDto>
            {
                Data = dataResult.Value,
                PaginationInfo = new PaginationInfo
                {
                    PageSize = limit,
                    CurrentPage = page,
                    TotalCount = totalCount
                }
            };

            return Results.Ok(pagedResponse);
        });

        groupBuilder.MapGet("/{idOrUsername}", async (
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
            ).ToHttpResult(context.Request.Path);
        });

        return groupBuilder;
    }
}