using System.Data;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateReleaseRequest"/> objects.
/// </summary>
public class CreateReleaseRequestValidator : AbstractValidator<CreateReleaseRequest>
{
    private readonly MusicServiceDbContext _dbContext;
    public CreateReleaseRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.ReleaseTypeSlug)
            .NotEmpty()
            .MustAsync(BeExistingReleaseTypeSlugAsync);

        RuleFor(x => x.ReleaseDate)
            .NotEmpty()
            .Must(BeDateString);

        RuleFor(x => x.CoverUrl).NotEmpty();

        RuleFor(x => x.ArtistIds).MustAsync(BeExistingArtistIdsAsync);

        RuleFor(x => x.Tracks).NotEmpty();
        RuleForEach(x => x.Tracks).SetValidator(x => new TrackInfoValidator(_dbContext, x));
    }

    public class TrackInfoValidator : AbstractValidator<CreateReleaseRequest.TrackInfo>
    {
        private readonly MusicServiceDbContext _dbContext;
        public TrackInfoValidator(
            MusicServiceDbContext dbContext,
            CreateReleaseRequest createReleaseRequest
        )
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.TagSlugs).MustAsync(BeExistingTagSlugsAsync);
            RuleFor(x => x.ArtistIds)
                .Must((ids) => BeInArtistsOfRelease(ids!, createReleaseRequest))
                .When(x => x.ArtistIds is not null)
                .WithMessage("Artist ids should be in the list of Release artists");
        }

        private bool BeInArtistsOfRelease(
            IEnumerable<Guid> artistIds,
            CreateReleaseRequest createReleaseRequest
        )
        {
            foreach (var artistId in artistIds)
            {
                if (!createReleaseRequest.ArtistIds.Contains(artistId))
                {
                    return false;
                }
            }
            return true;
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

    private bool BeDateString(string value)
    {
        return DateTime.TryParse(value, out var _);
    }
    private async Task<bool> BeExistingReleaseTypeSlugAsync(
        string releaseTypeSlug,
        CancellationToken cancellationToken
    )
    {
        var release = await _dbContext.ReleaseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug == releaseTypeSlug, cancellationToken);

        return release is not null;
    }
}