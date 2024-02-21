using FluentValidation;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

public sealed class CreateReleaseRequestTrackInfoValidator : AbstractValidator<CreateReleaseRequest.TrackInfo>
{
    public CreateReleaseRequestTrackInfoValidator(MusicServiceDbContext dbContext)
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.TagSlugs).MustAsync((slugs, cancel) => 
            RuleHelpers.BeExistingTagSlugsAsync(slugs, dbContext, cancel)
        );
        RuleFor(x => x.ArtistIds)
            .MustAsync((ids, cancel) => RuleHelpers.BeExistingArtistIdsAsync(ids!, dbContext, cancel))
            .When(x => x.ArtistIds is not null);
    }
}