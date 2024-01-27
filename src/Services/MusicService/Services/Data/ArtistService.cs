using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Errors;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="IArtistService"/>
public sealed class ArtistService : IArtistService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateArtistRequest> _createRequestValidator;
    public ArtistService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateArtistRequest> createRequestValidator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
    }

    public async Task<Result<Artist>> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO add main user info to db
        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create an Artist, incorrect data!",
                validationResult.Errors
            ).ToValueResult<Artist>();
        }

        var artistType = await _dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(at => at.Slug == request.ArtistTypeSlug, cancellationToken);
        if (artistType is null)
        {
            return new InternalError(
                "Could not create an Artist"
            ).ToValueResult<Artist>();
        }

        var slugResult = await GenerateSlugAsync(request.Name, cancellationToken);
        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<Artist>();
        }

        var artist = new Artist
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ArtistTypeId = artistType.Id,
            Slug = slugResult.Value,
            CoverUrl = request.CoverUrl,
            CreatorUserId = "NOT IMPLEMENTED", // TODO add implementation
        };
        artist.ArtistUsers = request.UserIds.Select(userId => new ArtistUser
        {
            ArtistId = artist.Id,
            UserId = userId
        }).ToList();

        await _dbContext.AddAsync(artist, cancellationToken);

        return artist.ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid artistId, CancellationToken cancellationToken = default)
    {
        var artist = await _dbContext.Artists
            .FirstOrDefaultAsync(a => a.Id == artistId, cancellationToken);
        if (artist is null)
        {
            return new NoContentError(
                "Could not able to delete artist, content not found."
            ).ToResult();
        }

        _dbContext.Artists.Remove(artist);

        return Result.Success();
    }

    public async Task<Result<Artist>> UpdateAsync(
        UpdateArtistRequest request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO add validator

        var artist = await _dbContext.Artists
            .Include(a => a.ArtistUsers)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (artist is null)
        {
            return new NotFoundError($"Artist with Id = {request.Id} not found")
                .ToValueResult<Artist>();
        }

        var artistType = await _dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(at => at.Slug == request.ArtistTypeSlug, cancellationToken);
        if (artistType is null)
        {
            return new InternalError(
                "Could not update an Artist"
            ).ToValueResult<Artist>();
        }

        artist.ArtistTypeId = artistType.Id;
        artist.CoverUrl = request.CoverUrl ?? artist.CoverUrl;

        if (request.Name is not null)
        {
            artist.Name = request.Name;

            var slugResult = await GenerateSlugAsync(request.Name, cancellationToken);
            if (slugResult.IsFailure)
            {
                return slugResult.Error.ToValueResult<Artist>();
            }
            artist.Slug = slugResult.Value;
        }

        if (request.UserIds is not null)
        {
            var updateResult = UpdateArtistUsers(artist, request.UserIds);
            if (updateResult.IsFailure)
            {
                return updateResult.Error.ToValueResult<Artist>();
            }
        }

        return artist.ToValueResult();
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalError(
                $"Cannot save changes to database: {ex.Message}"
            ).ToResult();
        }
    }

    public IQueryable<Artist> GetQueryable()
    {
        return _dbContext.Artists.AsNoTracking().AsQueryable();
    }

    /// <summary>
    ///     Updates <see cref="Artist.ArtistUsers"/> of passed user.
    /// </summary>
    /// <remarks>
    ///     Pass tracking entity, or changes would not apply. <br/>
    ///     Use <see cref="SaveChangesAsync"/> to save changes.
    /// </remarks>
    /// 
    /// <param name="artist">
    ///     Artist to change <see cref="Artist.ArtistUsers"/> of.
    /// </param>
    /// <param name="userIds">
    ///     Updated collection of user identifiers.
    /// </param>
    /// <returns></returns>
    private Result UpdateArtistUsers(Artist artist, IEnumerable<string> userIds)
    {
        if (artist is null || userIds is null)
        {
            return new InternalError(
                "Couldn't update artist users"
            ).ToResult();
        }

        try
        {
            var userIdsToAdd = userIds
                .Where(id => artist.ArtistUsers!.FirstOrDefault(au => au.UserId == id) is null)
                .ToArray();

            var artistUsersToDelete = artist.ArtistUsers!
                .Where(au => !userIds.Contains(au.UserId))
                .ToArray();
            foreach (var artistUser in artistUsersToDelete)
            {
                artist.ArtistUsers!.Remove(artistUser);
                _dbContext.ArtistUsers.Remove(artistUser);
            }
            foreach (var userId in userIdsToAdd)
            {
                artist.ArtistUsers!.Add(new()
                {
                    UserId = userId,
                    ArtistId = artist.Id
                });
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalError(
                $"Couldn't update artist users: {ex.Message}"
            ).ToResult();
        }
    }

    /// <summary>
    ///     Adds users to the <see cref="Artist"/> with passed Id into database. 
    /// </summary>
    /// <remarks>
    ///     Does not check if user exists in identity service, validators should check themselves. 
    /// </remarks>
    /// <param name="artistId">
    ///    The identifier of the <see cref="Artist"/> to add users.
    /// </param>
    /// <param name="userIds">
    ///     A collection of user identifiers to add. 
    /// </param>
    /// <param name="cancellationToken">
    ///     A token for cancelling operation. 
    /// </param>
    /// <returns>
    ///     A task representing asynchronous operation.
    /// </returns>
    private Task AddArtistUsersAsync(
        Guid artistId,
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.ArtistUsers.AddRangeAsync(
            userIds.Select(i => new ArtistUser
            {
                ArtistId = artistId,
                UserId = i
            }),
            cancellationToken
        );
    }


    /// <summary>
    ///     Generates slug from <see cref="CreateArtistRequest"/>.
    ///     Tries generate unique slug.
    /// </summary>
    /// 
    /// <param name="value">
    ///     The string to generate the slug for.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token for cancellation.
    /// </param>
    /// <returns>
    ///     The result object that contains a string value which is a generated slug.
    /// </returns>
    private async Task<Result<string>> GenerateSlugAsync(
        string value,
        CancellationToken cancellationToken
    )
    {
        var slugResult = _slugGenerator.Generate(value);

        if (slugResult.IsFailure)
        {
            return slugResult.Error.ToValueResult<string>();
        }

        try
        {
            var slug = slugResult.Value;
            var artistsWithSimilarSlug = await _dbContext.Artists
                .AsNoTracking()
                .Where(a => a.Slug.StartsWith(slug))
                .Select(a => a.Slug)
                .ToArrayAsync(cancellationToken);

            var suffixedSlug = slug;
            // Add number suffix until it is unique
            for (var i = 1; artistsWithSimilarSlug.Contains(suffixedSlug); i++)
            {
                suffixedSlug = slug + '-' + i;
            }

            return suffixedSlug.ToValueResult();
        }
        catch (Exception ex)
        {
            return new InternalError(
                $"Could not generate slug!: {ex.Message}"
            ).ToValueResult<string>();
        }
    }

}