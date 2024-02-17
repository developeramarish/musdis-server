using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     Validator for <see cref="UpdateArtistRequest"/> objects.
/// </summary>
public class UpdateArtistRequestValidator : AbstractValidator<UpdateArtistRequest>
{
    // TODO add user ids check.
    public UpdateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().When(x => x.Name is not null);

        RuleFor(x => x.ArtistTypeSlug!)
            .NotEmpty()
            .MustAsync((slug, cancel) => 
                RuleHelpers.BeExistingArtistTypeSlugAsync(slug, dbContext, cancel)
            )
            .When(x => x.ArtistTypeSlug is not null);
    }
}