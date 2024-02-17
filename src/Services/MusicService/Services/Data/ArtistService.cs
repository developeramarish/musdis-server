using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="IArtistService"/>
public sealed class ArtistService : IArtistService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateArtistRequest> _createRequestValidator;
    private readonly IValidator<UpdateArtistRequest> _updateRequestValidator;
    public ArtistService(
        MusicServiceDbContext dbContext,
        ISlugGenerator slugGenerator,
        IValidator<CreateArtistRequest> createRequestValidator,
        IValidator<UpdateArtistRequest> updateRequestValidator
    )
    {
        _dbContext = dbContext;
        _slugGenerator = slugGenerator;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<Result<Artist>> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var existingArtist = await _dbContext.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);
        if (existingArtist is not null)
        {
            return new ConflictError(
                $"Artist with name = {request.Name} exists"
            ).ToValueResult<Artist>();
        }

        // TODO add main user info to db 
        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not create an Artist, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<Artist>();
        }

        var artistType = await _dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(at => at.Slug == request.ArtistTypeSlug, cancellationToken);
        if (artistType is null)
        {
            return new InternalServerError(
                "Could not create an Artist"
            ).ToValueResult<Artist>();
        }

        var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Artist>(
            request.Name,
            cancellationToken
        );
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
            CreatorId = "NOT IMPLEMENTED", // TODO add implementation
        };
        artist.ArtistUsers = request.UserIds.Select(userId => new ArtistUser
        {
            ArtistId = artist.Id,
            UserId = userId,
            UserName = "" // TODO implement
        }).ToList();

        await _dbContext.Artists.AddAsync(artist, cancellationToken);
        await _dbContext.Entry(artist).Reference(a => a.ArtistType).LoadAsync(cancellationToken);

        return artist.ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid artistId, CancellationToken cancellationToken = default)
    {
        var artist = await _dbContext.Artists
            .FirstOrDefaultAsync(a => a.Id == artistId, cancellationToken);
        if (artist is null)
        {
            return new NoContentError(
                $"Could not able to delete artist, content with Id={artistId} not found."
            ).ToResult();
        }

        _dbContext.Artists.Remove(artist);

        return Result.Success();
    }

    public async Task<Result<Artist>> UpdateAsync(
        Guid id,
        UpdateArtistRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var artist = await _dbContext.Artists
            .Include(a => a.ArtistUsers)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (artist is null)
        {
            return new NotFoundError($"Artist with Id = {id} not found")
                .ToValueResult<Artist>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Could not update an Artist, incorrect data!",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<Artist>();
        }

        if (request.ArtistTypeSlug is not null)
        {
            var artistType = await _dbContext.ArtistTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(at => at.Slug == request.ArtistTypeSlug, cancellationToken);
            if (artistType is null)
            {
                return new InternalServerError(
                    "Could not update an Artist"
                ).ToValueResult<Artist>();
            }

            artist.ArtistTypeId = artistType.Id;
        }
        
        artist.CoverUrl = request.CoverUrl ?? artist.CoverUrl;
        if (request.Name is not null)
        {
            var slugResult = await _slugGenerator.GenerateUniqueSlugAsync<Artist>(
                request.Name,
                cancellationToken
            );
            if (slugResult.IsFailure)
            {
                return slugResult.Error.ToValueResult<Artist>();
            }
            artist.Name = request.Name;
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

    public IQueryable<Artist> GetQueryable()
    {
        return _dbContext.Artists.AsNoTracking();
    }

    /// <summary>
    ///     Updates <see cref="Artist.ArtistUsers"/> of passed user.
    /// </summary>
    /// <remarks>
    ///     Pass tracking entity, or changes would not apply. 
    /// </remarks>
    /// 
    /// <param name="artist">
    ///     Artist to change <see cref="Artist.ArtistUsers"/> of.
    /// </param>
    /// <param name="userIds">
    ///     Updated collection of user identifiers.
    /// </param>
    /// 
    /// <returns>
    ///     The result of an operation.
    /// </returns>
    private Result UpdateArtistUsers(Artist artist, IEnumerable<string> userIds)
    {
        if (artist?.ArtistUsers is null || userIds is null)
        {
            return new InternalServerError(
                "Couldn't update artist users"
            ).ToResult();
        }

        try
        {
            var userIdsToAdd = userIds
                .Where(id => artist.ArtistUsers.FirstOrDefault(au => au.UserId == id) is null)
                .ToArray();

            var artistUsersToDelete = artist.ArtistUsers
                .Where(au => !userIds.Contains(au.UserId))
                .ToArray();
            foreach (var artistUser in artistUsersToDelete)
            {
                artist.ArtistUsers!.Remove(artistUser);
                _dbContext.ArtistUsers.Remove(artistUser);
            }
            foreach (var userId in userIdsToAdd)
            {
                artist.ArtistUsers.Add(new()
                {
                    UserId = userId,
                    ArtistId = artist.Id,
                    UserName = "", // TODO implement
                });
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return new InternalServerError(
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
                UserId = i,
                UserName = "", // TODO implement
            }),
            cancellationToken
        );
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
            return new InternalServerError(
                $"Cannot save changes to database: {ex.Message}"
            ).ToResult();
        }
    }
}