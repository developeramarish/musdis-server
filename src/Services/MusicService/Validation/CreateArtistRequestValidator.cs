using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateArtistRequest"/> objects.
/// </summary>
public class CreateArtistRequestValidator : AbstractValidator<CreateArtistRequest>
{
    // TODO add user ids check.
    public CreateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ArtistTypeSlug)
            .NotEmpty()
            .MustAsync((slug, cancel) =>
                RuleHelpers.BeExistingArtistTypeSlugAsync(slug, dbContext, cancel)
            );
    }
}