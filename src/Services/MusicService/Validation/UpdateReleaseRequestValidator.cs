using FluentValidation;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for the <see cref="UpdateReleaseRequest"/>
/// </summary>
public sealed class UpdateReleaseRequestValidator : AbstractValidator<UpdateReleaseRequest>
{
    public UpdateReleaseRequestValidator(MusicServiceDbContext dbContext)
    {

        RuleFor(x => x.Name).NotEmpty().When(x => x.Name is not null);

        RuleFor(x => x.ReleaseDate)
            .Must(str => RuleHelpers.BeDateString(str!))
            .When(x => x.ReleaseDate is not null);

        RuleFor(x => x.ReleaseTypeSlug)
            .MustAsync((slug, cancel) =>
                RuleHelpers.BeExistingReleaseTypeSlugAsync(slug!, dbContext, cancel)
            )
            .When(x => x.ReleaseTypeSlug is not null);

        RuleFor(x => x.ArtistIds)
            .NotEmpty()
            .MustAsync((ids, cancel) =>
                RuleHelpers.BeExistingArtistIdsAsync(ids!, dbContext, cancel)
            )
            .When(x => x.ArtistIds is not null);

        RuleFor(x => x.CoverFile)
            .Must(x => RuleHelpers.BeValidUrl(x!.Url))
            .When(x => x.CoverFile is not null);
    }
}