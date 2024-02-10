using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="UpdateTrackRequest"/>.
/// </summary>
public class UpdateTrackRequestValidator : AbstractValidator<UpdateTrackRequest>
{
    private readonly MusicServiceDbContext _dbContext;
    public UpdateTrackRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Title).NotEmpty().When(x => x.Title is not null);
        RuleFor(x => x.TagSlugs)
            .MustAsync(BeExistingTagSlugsAsync)
            .When(x => x.TagSlugs is not null)
            .WithMessage("Tags with provided slugs do not exist.");

        RuleFor(x => x.ReleaseId)
            .MustAsync(BeExistingReleaseIdAsync)
            .When(x => x.ReleaseId is not null)
            .WithMessage(
                x => $"Cannot create Track with ReleaseId = {x.ReleaseId}, Release it is not found."
            );

        RuleFor(x => x.ArtistIds)
            .MustAsync(BeExistingArtistIdsAsync)
            .When(x => x.ArtistIds is not null)
            .WithMessage("Provided artist ids should be in database");
        RuleFor(x => x.ArtistIds)
            .MustAsync(BeInArtistsOfReleaseAsync)
            .When(x => x.ArtistIds is not null)
            .WithMessage(
                x => $"Provided artist ids do not match artists in Release with Id = {x.ReleaseId}."
            );
    }

    private async Task<bool> BeExistingTagSlugsAsync(
        IEnumerable<string>? tagSlugs,
        CancellationToken cancellationToken = default
    )
    {
        if (tagSlugs is null)
        {
            throw new ArgumentNullException(
                nameof(tagSlugs),
                "Tag slugs should not be null in validation method. Check FluentValidation conditions."
            );
        }

        var existingCount = await _dbContext.Tags
            .AsNoTracking()
            .Where(t => tagSlugs.Contains(t.Slug))
            .CountAsync(cancellationToken);

        return existingCount == tagSlugs.Count();
    }

    private async Task<bool> BeInArtistsOfReleaseAsync(
        UpdateTrackRequest request,
        IEnumerable<Guid>? artistIds,
        CancellationToken cancellationToken = default
    )
    {
        if (artistIds is null)
        {
            throw new ArgumentNullException(
                nameof(artistIds),
                "Artist ids should not be null in validation method. Check FluentValidation conditions."
            );
        }

        var release = await _dbContext.Releases
            .Include(r => r.Artists)
            .AsNoTracking()
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
        IEnumerable<Guid>? artistIds,
        CancellationToken cancellationToken = default
    )
    {
        if (artistIds is null)
        {
            throw new ArgumentNullException(
                nameof(artistIds),
                "Artist ids should not be null in validation method. Check FluentValidation conditions."
            );
        }

        var existingCount = await _dbContext.Artists
            .Where(a => artistIds.Contains(a.Id))
            .AsNoTracking()
            .CountAsync(cancellationToken);

        return existingCount == artistIds.Count();
    }

    private async Task<bool> BeExistingReleaseIdAsync(
        Guid? releaseId,
        CancellationToken cancellationToken = default
    )
    {
        if (releaseId is null)
        {
            throw new ArgumentNullException(
                nameof(releaseId),
                "Release id should not be null in validation method. Check FluentValidation conditions."
            );
        }

        var release = await _dbContext.Releases.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == releaseId,
            cancellationToken
        );

        return release is not null;
    }
}