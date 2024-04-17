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

/// <summary>
///     Artist endpoints.
/// </summary>
public static class ArtistEndpoints
{
    /// <summary>
    ///     Maps <see cref="Models.Artist"/> related endpoints.
    /// </summary>
    /// 
    /// <param name="groupBuilder">
    ///     The group to add endpoints to.
    /// </param>
    /// 
    /// <returns>
    ///     The <paramref name="groupBuilder"/> with mapped <see cref="Models.Artist"/> endpoints. 
    /// </returns>
    public static RouteGroupBuilder MapArtists(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<PagedDataResponse<ArtistDto>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<ArtistDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .Accepts<CreateArtistRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .Accepts<UpdateArtistRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }
    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] IArtistService artistService,
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 0
    )
    {
        var queryable = artistService.GetQueryable();
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
            .Include(a => a.ArtistUsers)
            .Include(a => a.ArtistType)
            .ToListAsync(cancellationToken);

        var totalCount = await queryable.CountAsync(cancellationToken);
        var result = new PagedDataResponse<ArtistDto>
        {
            Data = ArtistDto.FromArtists(artists),
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
        [FromServices] IArtistService artistService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var queryable = artistService.GetQueryable()
            .Include(a => a.ArtistUsers)
            .Include(a => a.ArtistType);

        var artist = await queryable
            .FirstOrDefaultAsync(a => a.Slug == idOrSlug, cancellationToken);
        if (artist is null && Guid.TryParse(idOrSlug, out var id))
        {
            artist = await queryable
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        if (artist is null)
        {
            return new NotFoundError(
                $"Cannot find Artist with Id or Slug = {{{idOrSlug}}}"
            ).ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<ArtistDto>
        {
            Data = ArtistDto.FromArtist(artist)
        });
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateArtistRequest request,
        [FromServices] IArtistService artistService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await artistService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await artistService.SaveChangesAsync(cancellationToken);
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
        [FromBody] UpdateArtistRequest request,
        [FromServices] IArtistService artistService,
        [FromServices] IAuthorizationService authorizationService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var artist = await artistService.GetQueryable()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (artist is null)
        {
            return new NotFoundError(
                $"Artist with Id = {{{id}}} not found"
            ).ToHttpResult(context.Request.Path);
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            artist,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Artist"
            ).ToHttpResult(context.Request.Path);
        }

        var updateResult = await artistService.UpdateAsync(id, request, cancellationToken);
        if (updateResult.IsFailure)
        {
            return updateResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await artistService.SaveChangesAsync(cancellationToken);
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
        [FromServices] IArtistService artistService,
        [FromServices] IAuthorizationService authorizationService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var artist = await artistService.GetQueryable()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (artist is null)
        {
            return new NoContentError().ToHttpResult(context.Request.Path);
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            artist,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Artist"
            ).ToHttpResult(context.Request.Path);
        }

        var fileId = artist.CoverFileId;

        var deleteResult = await artistService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }

        var saveResult = await artistService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }
        await publishEndpoint.Publish(new EntityWithFileDeleted(fileId));

        return Results.Ok();
    }
}