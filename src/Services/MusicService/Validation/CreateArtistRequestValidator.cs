using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     Validator for <see cref="CreateArtistRequest"/> objects.
/// </summary>
public class CreateArtistRequestValidator : AbstractValidator<CreateArtistRequest>
{
    // TODO add user ids check.
    private readonly MusicServiceDbContext _dbContext;
    public CreateArtistRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ArtistTypeSlug)
            .NotEmpty()
            .MustAsync(BeExistingArtistTypeSlugAsync);
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