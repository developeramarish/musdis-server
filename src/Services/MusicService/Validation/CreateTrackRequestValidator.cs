using System.Data;

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
    public CreateTrackRequestValidator(MusicServiceDbContext dbContext)
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ReleaseId)
            .MustAsync((id, cancel) =>
                RuleHelpers.BeExistingReleaseIdAsync(id, dbContext, cancel)
            )
            .WithMessage(
                x => $"Cannot create Track with ReleaseId = {{{x.ReleaseId}}}, Release it is not found."
            );

        RuleFor(x => x.ArtistIds)
            .MustAsync((ids, cancel) =>
                RuleHelpers.BeExistingArtistIdsAsync(ids, dbContext, cancel)
            )
            .WithMessage("Provided artist ids should be in database");

        RuleFor(x => x.TagSlugs)
            .MustAsync((slugs, cancel) =>
                RuleHelpers.BeExistingTagSlugsAsync(slugs, dbContext, cancel)
            )
            .WithMessage("Tags with provided slugs do not exist.");
    }
}