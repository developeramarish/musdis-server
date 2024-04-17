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

public static class TrackEndpoints
{
    public static RouteGroupBuilder MapTracks(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", HandleGetManyAsync)
            .Produces<PagedDataResponse<IEnumerable<TrackDto>>>();

        groupBuilder.MapGet("/{idOrSlug}", HandleGetOneAsync)
            .Produces<DataResponse<TrackDto>>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", HandlePostAsync)
            .Accepts<CreateTrackRequest>(MediaTypeNames.Application.Json)
            .Produces<TrackDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", HandlePatchAsync)
            .Accepts<UpdateTrackRequest>(MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        groupBuilder.MapDelete("/{id:guid}", HandleDeleteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

        return groupBuilder;
    }

    public static async Task<IResult> HandleGetManyAsync(
        [FromServices] ITrackService trackService,
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 0
    )
    {
        var queryable = trackService.GetQueryable();
        if (search is not null)
        {
            queryable = queryable.Where(a => a.Title.StartsWith(search));
        }
        if (pageSize > 0)
        {
            var offset = (page - 1) * pageSize;
            queryable = queryable.Skip(offset).Take(pageSize);
        }

        var tracks = await queryable
            .Include(t => t.Artists)
            .Include(t => t.Tags)
            .Include(t => t.Release)
            .ToListAsync(cancellationToken);
        var totalCount = await queryable.CountAsync(cancellationToken);

        var data = TrackDto.FromTracks(tracks);

        return Results.Ok(new PagedDataResponse<TrackDto>
        {
            Data = data,
            PaginationInfo = new()
            {
                PageSize = pageSize,
                TotalCount = totalCount,
                CurrentPage = page
            }
        });
    }

    public static async Task<IResult> HandleGetOneAsync(
        [FromRoute] string idOrSlug,
        [FromServices] ITrackService trackService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var queryable = trackService.GetQueryable()
            .Include(t => t.Artists)
            .Include(t => t.Tags)
            .Include(t => t.Release);

        var track = await queryable
            .FirstOrDefaultAsync(t => t.Slug == idOrSlug, cancellationToken);

        if (track is null && Guid.TryParse(idOrSlug, out var id))
        {
            track = await queryable
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        if (track is null)
        {
            return new NotFoundError(
                $"Track with Id or Slug = {{{idOrSlug}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        var trackDto = TrackDto.FromTrack(track);
        return Results.Ok(new DataResponse<TrackDto>
        {
            Data = trackDto
        });
    }

    public static async Task<IResult> HandlePostAsync(
        [FromBody] CreateTrackRequest request,
        [FromServices] ITrackService trackService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var createResult = await trackService.CreateAsync(request, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await trackService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        await publishEndpoint.Publish(new FileUsed(request.AudioFile.Id));

        var dto = createResult.Value;

        return Results.Created($"{context.Request.Path}/{dto.Id}", dto);
    }

    public static async Task<IResult> HandlePatchAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateTrackRequest request,
        [FromServices] ITrackService trackService,
        [FromServices] IPublishEndpoint publishEndpoint,
        [FromServices] IAuthorizationService authorizationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var track = await trackService
            .GetQueryable()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (track is null)
        {
            return new NotFoundError(
                $"Track with Id = {{{id}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            track,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Track"
            ).ToHttpResult(context.Request.Path);
        }

        var updateResult = await trackService.UpdateAsync(id, request, cancellationToken);
        if (updateResult.IsFailure)
        {
            return updateResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await trackService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        if (request.AudioFile is not null)
        {
            await publishEndpoint.Publish(new FileUsed(request.AudioFile.Id));
        }

        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAsync(
        [FromRoute] Guid id,
        [FromServices] ITrackService trackService,
        [FromServices] IAuthorizationService authorizationService,
        [FromServices] IPublishEndpoint publishEndpoint,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var track = await trackService
            .GetQueryable()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (track is null)
        {
            return new NotFoundError(
                $"Track with Id = {{{id}}} is not found."
            ).ToHttpResult(context.Request.Path);
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            context.User,
            track,
            AuthorizationPolicies.SameAuthor
        );
        if (!authorizationResult.Succeeded)
        {
            return new ForbiddenError(
                "You are not authorized to update this Release"
            ).ToHttpResult(context.Request.Path);
        }

        var fileId = track.AudioFileId;

        var deleteResult = await trackService.DeleteAsync(id, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToHttpResult(context.Request.Path);
        }
        var saveResult = await trackService.SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error.ToHttpResult(context.Request.Path);
        }

        await publishEndpoint.Publish(new FileUsed(fileId));

        return Results.NoContent();
    }
}