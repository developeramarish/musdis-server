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
    private readonly MusicServiceDbContext _dbContext;
    public UpdateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty().When(x => x.Name is not null);

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .When(x => x.Name is not null)
            .WithErrorCode(ErrorCodes.NonUniqueData)
            .WithMessage("Artist name must be unique.");

        RuleFor(x => x.CoverFile)
            .Must(x => RuleHelpers.BeValidUrl(x!.Url))
            .When(x => x.CoverFile is not null);

        RuleFor(x => x.ArtistTypeSlug!)
            .NotEmpty()
            .MustAsync((slug, cancel) =>
                RuleHelpers.BeExistingArtistTypeSlugAsync(slug, dbContext, cancel)
            )
            .When(x => x.ArtistTypeSlug is not null);
    }

    private async Task<bool> BeUniqueNameAsync(
        string? name,
        CancellationToken cancellationToken = default
    )
    {
        var releaseType = await _dbContext.Artists.FirstOrDefaultAsync(
            a => a.Name == name,
            cancellationToken
        );

        return releaseType is null;
    }
}