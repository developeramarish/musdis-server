using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Dtos;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Data;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

namespace Musdis.MusicService.Endpoints;

/// <summary>
///     Artists endpoints.
/// </summary>
public static class ArtistEndpoints
{
    public static RouteGroupBuilder MapArtists(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapGet("/", async (
            HttpContext context,
            CancellationToken cancellationToken,
            [FromServices] IArtistService artistService,
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 0
        ) =>
        {
            var queryable = artistService
                .GetQueryable()
                .Include(a => a.ArtistUsers)
                .Include(a => a.ArtistType)
                .AsQueryable();
            if (search is not null)
            {
                queryable = queryable.Where(a => a.Name.StartsWith(search));
            }

            var offset = (page - 1) * limit;
            var artists = await queryable
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            var dataResult = ArtistDto.FromArtists(artists);
            if (dataResult.IsFailure)
            {
                return dataResult.Error.ToHttpResult(context.Request.Path);
            }

            var totalCount = queryable.Count();
            var result = new PagedDataResponse<ArtistDto>
            {
                Data = dataResult.Value,
                PaginationInfo = new()
                {
                    PageSize = limit,
                    TotalCount = totalCount,
                    CurrentPage = page - 1
                }
            };

            return Results.Ok(result);
        }).Produces<PagedDataResponse<ArtistDto>>();

        groupBuilder.MapGet("/{id:guid}", async (
            Guid id,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromServices] IArtistService artistService
        ) =>
        {
            var artist = await artistService.GetQueryable()
                .Include(a => a.ArtistUsers)
                .Include(a => a.ArtistType)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (artist is null)
            {
                return new NotFoundError(
                    $"Cannot find Artist with Id = {id}"
                ).ToHttpResult(context.Request.Path);
            }

            var dtoResult = ArtistDto.FromArtist(artist);
            if (dtoResult.IsFailure)
            {
                return new InternalServerError(
                    "Cannot convert artist to DTO."
                ).ToHttpResult(context.Request.Path);
            }

            return Results.Ok(dtoResult.Value);
        })
            .Produces<ArtistDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        groupBuilder.MapPost("/", async (
            CreateArtistRequest request,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromServices] IArtistService artistService
        ) =>
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

            var artist = createResult.Value;
            var artistDto = ArtistDto.FromArtist(artist);

            return Results.Created($"{context.Request.Path}/{artist.Id}", artistDto.Value);
        })
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        groupBuilder.MapPatch("/{id:guid}", async (
            Guid id,
            UpdateArtistRequest request,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromServices] IArtistService artistService
        ) =>
        {
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

            return Results.NoContent();
        })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return groupBuilder;
    }
}