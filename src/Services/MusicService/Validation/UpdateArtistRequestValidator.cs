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
    private readonly MusicServiceDbContext _dbContext;
    public UpdateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty().When(x => x.Name is not null);

        RuleFor(x => x.ArtistTypeSlug!)
            .NotEmpty()
            .MustAsync(BeExistingArtistTypeSlugAsync)
            .When(x => x.ArtistTypeSlug is not null);
    }

    private async Task<bool> BeExistingArtistTypeSlugAsync(
        string artistTypeSlug,
        CancellationToken cancellationToken
    )
    {
        var artist = await _dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug == artistTypeSlug, cancellationToken);

        return artist is not null;
    }
}