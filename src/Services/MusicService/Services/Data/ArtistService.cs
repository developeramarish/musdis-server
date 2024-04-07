using System.IdentityModel.Tokens.Jwt;

using FluentValidation;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Musdis.MessageBrokerHelpers.Events;
using Musdis.MusicService.Data;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Models;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Grpc;
using Musdis.MusicService.Services.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Data;

/// <inheritdoc cref="IArtistService"/>
public sealed class ArtistService : IArtistService
{
    private readonly MusicServiceDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentityUserService _identityUserService;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IValidator<CreateArtistRequest> _createRequestValidator;
    private readonly IValidator<UpdateArtistRequest> _updateRequestValidator;

    public ArtistService(
        MusicServiceDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ISlugGenerator slugGenerator,
        IIdentityUserService identityUserService,
        IValidator<CreateArtistRequest> createRequestValidator,
        IValidator<UpdateArtistRequest> updateRequestValidator
    )
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _slugGenerator = slugGenerator;
        _identityUserService = identityUserService;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<Result<Artist>> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userId is null)
        {
            return new UnauthorizedError(
                "Cannot create an Artist without a valid User"
            ).ToValueResult<Artist>();
        }

        var validationResult = await _createRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToError(
                "Cannot create an Artist, incorrect data."
            ).ToValueResult<Artist>();
        }

        var userInfosResult = await _identityUserService.GetUserInfosAsync(
            [userId, .. request.UserIds],
            cancellationToken
        );
        if (userInfosResult.IsFailure)
        {
            return userInfosResult.Error.ToValueResult<Artist>();
        }

        var artistType = await _dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(at => at.Slug == request.ArtistTypeSlug, cancellationToken);
        if (artistType is null)
        {
            return new InternalServerError(
                "Cannot create an Artist."
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
            CoverUrl = request.CoverFile.Url,
            CoverFileId = request.CoverFile.Id,
            CreatorId = userId,
            ArtistUsers = []
        };

        var users = userInfosResult.Value.Users;
        foreach (var user in users)
        {
            artist.ArtistUsers.Add(new ArtistUser
            {
                ArtistId = artist.Id,
                UserId = user.Id,
                UserName = user.UserName
            });
        }

        await _dbContext.Artists.AddAsync(artist, cancellationToken);
        await _dbContext.Entry(artist).Reference(a => a.ArtistType).LoadAsync(cancellationToken);
        await _dbContext.Entry(artist).Collection(a => a.ArtistUsers!).LoadAsync(cancellationToken);

        return artist.ToValueResult();
    }

    public async Task<Result> DeleteAsync(Guid artistId, CancellationToken cancellationToken = default)
    {
        var artist = await _dbContext.Artists
            .FirstOrDefaultAsync(a => a.Id == artistId, cancellationToken);
        if (artist is null)
        {
            return new NoContentError(
                $"Could not able to delete artist, content with Id = {{{artistId}}} not found."
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
            return new NotFoundError($"Artist with Id = {{{id}}} not found")
                .ToValueResult<Artist>();
        }

        var validationResult = await _updateRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToError(
                "Cannot update artist, incorrect data."
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

        if (request.CoverFile is not null)
        {
            artist.CoverUrl = request.CoverFile.Url;
            artist.CoverFileId = request.CoverFile.Id;
        }
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
            var updateResult = await UpdateArtistUsersAsync(artist, request.UserIds, cancellationToken);
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
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// 
    /// <returns>
    ///     A task representing asynchronous operation.
    ///     The task result contains <see cref="Result"/> of the operation.
    /// </returns>
    private async Task<Result> UpdateArtistUsersAsync(
        Artist artist,
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default
    )
    {
        if (artist?.ArtistUsers is null || userIds is null)
        {
            return new InternalServerError(
                "Couldn't update artist users"
            ).ToResult();
        }

        var userInfosResult = await _identityUserService.GetUserInfosAsync(userIds, cancellationToken);
        if (userInfosResult.IsFailure)
        {
            return userInfosResult.Error.ToResult();
        }

        try
        {
            var users = userInfosResult.Value.Users;

            var artistUsersToDelete = artist.ArtistUsers
                .ExceptBy(userIds, au => au.UserId)
                .ToArray();
            var userIdsToAdd = userIds
                .Except(artist.ArtistUsers.Select(u => u.UserId))
                .ToArray();

            foreach (var artistUser in artistUsersToDelete)
            {
                artist.ArtistUsers!.Remove(artistUser);
                _dbContext.ArtistUsers.Remove(artistUser);
            }
            foreach (var userId in userIdsToAdd)
            {
                var userName = users.FirstOrDefault(u => u.Id == userId)?.UserName;
                if (userName is null)
                {
                    return new InternalServerError(
                        "Cannot update artist users"
                    ).ToResult();
                }

                artist.ArtistUsers.Add(new()
                {
                    UserId = userId,
                    ArtistId = artist.Id,
                    UserName = userName,
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