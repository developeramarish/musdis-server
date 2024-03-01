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
///     Release types endpoints.
/// </summary>
public static class ReleaseTypeEndpoints
{
    public static RouteGroupBuilder MapReleaseTypes(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<PagedDataResponse<ReleaseTypeDto>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<ArtistDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<CreateReleaseTypeRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<UpdateReleaseTypeRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }

    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] IReleaseTypeService releaseTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var data = await releaseTypeService.GetQueryable()
            .ToArrayAsync(cancellationToken);

        return Results.Ok(new DataResponse<IEnumerable<ReleaseTypeDto>>
        {
            Data = data.Select(ReleaseTypeDto.FromReleaseType)
        });
    }

    public static async Task<IResult> HandleGetOneAsync(
        [FromRoute] string idOrSlug,
        [FromServices] IReleaseTypeService releaseTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var releaseType = await releaseTypeService.GetQueryable()
            .FirstOrDefaultAsync(rt => rt.Slug == idOrSlug, cancellationToken);

        if (releaseType is null && Guid.TryParse(idOrSlug, out var id))
        {
            releaseType = await releaseTypeService.GetQueryable()
                .FirstOrDefaultAsync(rt => rt.Id == id, cancellationToken);
        }
        if (releaseType is null)
        {
            return new NotFoundError(
                $"Release type with Id or Slug = {{{idOrSlug}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<ReleaseTypeDto>
        {
            Data = ReleaseTypeDto.FromReleaseType(releaseType)
        });
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateReleaseTypeRequest request,
        [FromServices] IReleaseTypeService releaseTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await releaseTypeService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await releaseTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        var dto = ReleaseTypeDto.FromReleaseType(createResult.Value);

        return Results.Created($"{context.Request.Path}/{dto.Id}", dto);
    }

    public static async Task<IResult> HandlePatchAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReleaseTypeRequest request,
        [FromServices] IReleaseTypeService releaseTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var patchResult = await releaseTypeService.UpdateAsync(id, request, cancellationToken);
        if (patchResult.IsFailure)
        {
            return patchResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await releaseTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAsync(
        [FromRoute] Guid id,
        [FromServices] IReleaseTypeService releaseTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var deleteResult = await releaseTypeService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await releaseTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.Ok();
    }
}
