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
    public UpdateTrackRequestValidator(MusicServiceDbContext dbContext)
    {
        RuleFor(x => x.Title).NotEmpty().When(x => x.Title is not null);
        RuleFor(x => x.TagSlugs)
            .MustAsync((slugs, cancel) => 
                RuleHelpers.BeExistingTagSlugsAsync(slugs!, dbContext, cancel)
            )
            .When(x => x.TagSlugs is not null)
            .WithMessage("Tags with provided slugs do not exist.");

        RuleFor(x => x.ReleaseId)
            .MustAsync((id, cancel) => 
                RuleHelpers.BeExistingReleaseIdAsync(id!.Value, dbContext, cancel)
            )
            .When(x => x.ReleaseId is not null)
            .WithMessage(
                x => $"Cannot create Track with ReleaseId = {{{x.ReleaseId}}}, Release it is not found."
            );

        RuleFor(x => x.ArtistIds)
            .MustAsync((ids, cancel) => RuleHelpers.BeExistingArtistIdsAsync(ids!, dbContext, cancel))
            .When(x => x.ArtistIds is not null)
            .WithMessage("Provided artist ids should be in database");
    }
}