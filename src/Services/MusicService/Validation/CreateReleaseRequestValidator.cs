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
    public CreateReleaseRequestValidator(MusicServiceDbContext dbContext)
    {

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.ReleaseTypeSlug)
            .NotEmpty()
            .MustAsync((slug, cancel) =>
                RuleHelpers.BeExistingReleaseTypeSlugAsync(slug, dbContext, cancel)
            );

        RuleFor(x => x.ReleaseDate)
            .NotEmpty()
            .Must(RuleHelpers.BeDateString);

        RuleFor(x => x.CoverUrl).NotEmpty();

        RuleFor(x => x.ArtistIds)
            .MustAsync((ids, cancel) =>
                RuleHelpers.BeExistingArtistIdsAsync(ids, dbContext, cancel)
            );

        RuleFor(x => x.Tracks).NotEmpty();
        RuleForEach(x => x.Tracks).ChildRules(v =>
        {
            v.RuleFor(x => x.TagSlugs)
                .MustAsync((slug, cancel) =>
                    RuleHelpers.BeExistingTagSlugsAsync(slug, dbContext, cancel)
                )
                .WithMessage("Tags with provided identifiers must exist in the database.");
            v.RuleFor(x => x.ArtistIds)
                .MustAsync((ids, cancel) =>
                    RuleHelpers.BeExistingArtistIdsAsync(ids!, dbContext, cancel)
                )
                .When(x => x.ArtistIds is not null)
                .WithMessage("Artists with provided identifiers must exist in the database.");
        });
    }
}