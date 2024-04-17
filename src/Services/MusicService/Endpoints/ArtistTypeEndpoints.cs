using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Defaults;
using Musdis.MusicService.Dtos;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Data;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

namespace Musdis.MusicService.Endpoints;

/// <summary>
///     Artist type endpoints.
/// </summary>
public static class ArtistTypeEndpoints
{
    public static RouteGroupBuilder MapArtistTypes(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<DataResponse<IEnumerable<ArtistTypeDto>>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<DataResponse<ArtistTypeDto>>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<CreateArtistTypeRequest>(MediaTypeNames.Application.Json)
            .Produces<ArtistTypeDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Accepts<UpdateArtistTypeRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }

    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] IArtistTypeService artistTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var data = await artistTypeService.GetQueryable()
            .ToArrayAsync(cancellationToken);

        return Results.Ok(new DataResponse<IEnumerable<ArtistTypeDto>>
        {
            Data = ArtistTypeDto.FromArtistTypes(data)
        });
    }

    public static async Task<IResult> HandleGetOneAsync(
        [FromRoute] string idOrSlug,
        [FromServices] IArtistTypeService artistTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var artistType = await artistTypeService.GetQueryable()
            .FirstOrDefaultAsync(a => a.Slug == idOrSlug, cancellationToken);

        if (artistType is null && Guid.TryParse(idOrSlug, out var id))
        {
            artistType = await artistTypeService.GetQueryable()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
        if (artistType is null)
        {
            return new NotFoundError(
                $"Artist type with Id or Slug = {{{idOrSlug}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<ArtistTypeDto>
        {
            Data = ArtistTypeDto.FromArtistType(artistType)
        });
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateArtistTypeRequest request,
        [FromServices] IArtistTypeService artistTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await artistTypeService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await artistTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        var dto = createResult.Value;

        return Results.Created($"{context.Request.Path}/{dto.Id}", dto);
    }

    public static async Task<IResult> HandlePatchAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateArtistTypeRequest request,
        [FromServices] IArtistTypeService artistTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var patchResult = await artistTypeService.UpdateAsync(id, request, cancellationToken);
        if (patchResult.IsFailure)
        {
            return patchResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await artistTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAsync(
        [FromRoute] Guid id,
        [FromServices] IArtistTypeService artistTypeService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var deleteResult = await artistTypeService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await artistTypeService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        return Results.Ok();
    }
}