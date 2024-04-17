using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Defaults;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Data;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

namespace Musdis.MusicService.Endpoints;

/// <summary>
///     Tag endpoints.
/// </summary>
public static class TagEndpoints
{
    public static RouteGroupBuilder MapTags(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<PagedDataResponse<IEnumerable<TagDto>>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<DataResponse<TagDto>>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<CreateTagRequest>(MediaTypeNames.Application.Json)
            .Produces<TagDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<UpdateTagRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }

    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] ITagService tagService,
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 0
    )
    {
        var queryable = tagService.GetQueryable();
        if (search is not null)
        {
            queryable = queryable.Where(a => a.Name.StartsWith(search));
        }

        if (pageSize > 0)
        {
            var offset = (page - 1) * pageSize;
            queryable = queryable
                .Skip(offset)
                .Take(pageSize);
        }

        var tags = await queryable.ToListAsync(cancellationToken);

        var totalCount = await queryable.CountAsync(cancellationToken);
        var result = new PagedDataResponse<TagDto>
        {
            Data = TagDto.FromTags(tags),
            PaginationInfo = new()
            {
                PageSize = pageSize,
                TotalCount = totalCount,
                CurrentPage = page
            }
        };

        return Results.Ok(result);
    }

    public static async Task<IResult> HandleGetOneAsync(
        [FromRoute] string idOrSlug,
        [FromServices] ITagService tagService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var tag = await tagService.GetQueryable()
            .FirstOrDefaultAsync(t => t.Slug == idOrSlug, cancellationToken);
        
        if (tag is null && Guid.TryParse(idOrSlug, out var id))
        {
            tag = await tagService.GetQueryable()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
        if (tag is null)
        {
            return new NotFoundError(
                $"Tag with Id or Slug = {{{idOrSlug}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<TagDto>
        {
            Data = TagDto.FromTag(tag)
        });
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateTagRequest request,
        [FromServices] ITagService tagService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await tagService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await tagService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        var dto = createResult.Value;

        return Results.Created($"{context.Request.Path}/{dto.Id}", dto);
    }

    public static async Task<IResult> HandlePatchAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateTagRequest request,
        [FromServices] ITagService tagService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var updateResult = await tagService.UpdateAsync(id, request, cancellationToken);
        if (updateResult.IsFailure)
        {
            return updateResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await tagService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAsync(
        [FromRoute] Guid id,
        [FromServices] ITagService tagService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var deleteResult = await tagService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await tagService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.Ok();
    }
}
