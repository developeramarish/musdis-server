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
    private readonly MusicServiceDbContext _dbContext;
    public CreateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithErrorCode(ErrorCodes.NonUniqueData)
            .WithMessage("Artist name must be unique.");

        RuleFor(x => x.ArtistTypeSlug)
            .NotEmpty()
            .MustAsync((slug, cancel) =>
                RuleHelpers.BeExistingArtistTypeSlugAsync(slug, dbContext, cancel)
            );
    }

    private async Task<bool> BeUniqueNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var artist = await _dbContext.Artists.FirstOrDefaultAsync(
            a => a.Name == name,
            cancellationToken
        );

        return artist is null;
    }
}