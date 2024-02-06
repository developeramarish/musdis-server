using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateTrackRequest"/>.
/// </summary>
public class CreateTrackRequestValidator : AbstractValidator<CreateTrackRequest>
{
    private readonly IMusicServiceDbContext _dbContext;
    public CreateTrackRequestValidator(IMusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ReleaseId)
            .MustAsync(BeExistingReleaseIdAsync)
            .WithMessage(
                x => $"Cannot create Track with ReleaseId = {x.ReleaseId}, Release it is not found."
            );

        RuleFor(x => x.ArtistIds)
            .MustAsync(BeExistingArtistIdsAsync)
            .WithMessage("Provided artist ids should be in database");
        RuleFor(x => x.ArtistIds)
            .MustAsync(BeInArtistsOfReleaseAsync)
            .WithMessage(
                x => $"Provided artist ids do not match artists in Release with Id = {x.ReleaseId}."
            );

        RuleFor(x => x.TagSlugs)
            .MustAsync(BeExistingTagSlugsAsync)
            .WithMessage("Tags with provided slugs do not exist.");
    }

    private async Task<bool> BeExistingTagSlugsAsync(
        IEnumerable<string> tagSlugs,
        CancellationToken cancellationToken = default
    )
    {
        var existingCount = await _dbContext.Tags
            .AsNoTracking()
            .Where(t => tagSlugs.Contains(t.Slug))
            .CountAsync(cancellationToken);

        return existingCount == tagSlugs.Count();
    }

    private async Task<bool> BeInArtistsOfReleaseAsync(
        CreateTrackRequest request,
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken = default
    )
    {
        var release = await _dbContext.Releases
            .AsNoTracking()
            .Include(r => r.Artists)
            .FirstOrDefaultAsync(r => r.Id == request.ReleaseId, cancellationToken);
        if (release is null)
        {
            return false;
        }

        var releaseArtistIds = release.Artists!.Select(a => a.Id).ToList();
        foreach (var artistId in artistIds)
        {
            if (!releaseArtistIds.Contains(artistId))
            {
                return false;
            }
        }

        return true;
    }
    private async Task<bool> BeExistingArtistIdsAsync(
        IEnumerable<Guid> artistIds,
        CancellationToken cancellationToken = default
    )
    {
        var existingCount = await _dbContext.Artists
            .AsNoTracking()
            .Where(a => artistIds.Contains(a.Id))
            .CountAsync(cancellationToken);

        return existingCount == artistIds.Count();
    }

    private async Task<bool> BeExistingReleaseIdAsync(
        Guid releaseId,
        CancellationToken cancellationToken = default
    )
    {
        var release = await _dbContext.Releases.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == releaseId,
            cancellationToken
        );

        return release is not null;
    }
}