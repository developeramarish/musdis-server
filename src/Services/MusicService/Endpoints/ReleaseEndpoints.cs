using System.Net.Mime;

using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MessageBrokerHelpers.Events;
using Musdis.MusicService.Defaults;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Data;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

namespace Musdis.MusicService.Endpoints;

public static class ReleaseEndpoints
{
    public static RouteGroupBuilder MapReleases(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<PagedDataResponse<IEnumerable<ReleaseDto>>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<DataResponse<ReleaseDto>>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .Accepts<CreateReleaseRequest>(MediaTypeNames.Application.Json)
            .Produces<ReleaseDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .Accepts<UpdateReleaseRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }

    public static async Task<IResult> HandleGetOneAsync(
        [FromRoute] string idOrSlug,
        [FromServices] IReleaseService releaseService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var queryable = releaseService.GetQueryable()
            .Include(r => r.ReleaseType)
            .Include(r => r.Artists!)
                .ThenInclude(a => a.ArtistType)
            .Include(r => r.Artists!)
                .ThenInclude(a => a.ArtistUsers)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Tags)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Artists!)
                    .ThenInclude(a => a.ArtistType)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Artists!)
                    .ThenInclude(a => a.ArtistUsers)
            .AsSplitQuery();

        var release = await queryable
            .FirstOrDefaultAsync(r => r.Slug == idOrSlug, cancellationToken);
        if (release is null && Guid.TryParse(idOrSlug, out var id))
        {
            release = await queryable
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
        if (release is null)
        {
            return new NotFoundError(
                $"Release with Id or Slug = {{{idOrSlug}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<ReleaseDto>
        {
            Data = ReleaseDto.FromRelease(release)
        });
    }

    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] IReleaseService releaseService,
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 0
    )
    {
        var queryable = releaseService.GetQueryable();
        if (search is not null)
        {
            queryable = queryable.Where(a => a.Name.StartsWith(search));
        }

        if (pageSize > 0)
        {
            var offset = (page - 1) * pageSize;
            queryable = queryable.Skip(offset).Take(pageSize);
        }

        var artists = await queryable
            .Include(r => r.ReleaseType)
            .Include(r => r.Artists!)
                .ThenInclude(a => a.ArtistType)
            .Include(r => r.Artists!)
                .ThenInclude(a => a.ArtistUsers)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Tags)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Artists!)
                    .ThenInclude(a => a.ArtistType)
            .Include(r => r.Tracks!)
                .ThenInclude(t => t.Artists!)
                    .ThenInclude(a => a.ArtistUsers)
            .ToListAsync(cancellationToken);

        var totalCount = await queryable.CountAsync(cancellationToken);
        var result = new PagedDataResponse<ReleaseDto>
        {
            Data = ReleaseDto.FromReleases(artists),
            PaginationInfo = new()
            {
                PageSize = pageSize,
                TotalCount = totalCount,
                CurrentPage = page
            }
        };

        return Results.Ok(result);
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateReleaseRequest request,
        [FromServices] IReleaseService releaseService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await releaseService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await releaseService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        await publishEndpoint.Publish(new FileUsed(request.CoverFile.Id));

        var dto = createResult.Value;

        return Results.Created($"{context.Request.Path}/{dto.Id}", dto);
    }

    public static async Task<IResult> HandlePatchAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReleaseRequest request,
        [FromServices] IReleaseService releaseService,
        [FromServices] IPublishEndpoint publishEndpoint,
        [FromServices] IAuthorizationService authorizationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var release = await releaseService
            .GetQueryable()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (release is null)
        {
            return new NotFoundError(
                $"Release with Id = {{{id}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            release,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Release"
            ).ToHttpResult(context.Request.Path);
        }

        var updateResult = await releaseService.UpdateAsync(id, request, cancellationToken);
        if (updateResult.IsFailure)
        {
            return updateResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await releaseService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        if (request.CoverFile is not null)
        {
            await publishEndpoint.Publish(new FileUsed(request.CoverFile.Id));
        }

        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAsync(
        [FromRoute] Guid id,
        [FromServices] IReleaseService releaseService,
        [FromServices] IPublishEndpoint publishEndpoint,
        [FromServices] IAuthorizationService authorizationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var release = await releaseService
            .GetQueryable()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (release is null)
        {
            return new NoContentError().ToHttpResult(context.Request.Path);
        }


        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            release,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Release"
            ).ToHttpResult(context.Request.Path);
        }

        var fileId = release.CoverFileId;
        
        var deleteResult = await releaseService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await releaseService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        await publishEndpoint.Publish(new EntityWithFileDeleted(fileId));


        return Results.Ok();
    }
}