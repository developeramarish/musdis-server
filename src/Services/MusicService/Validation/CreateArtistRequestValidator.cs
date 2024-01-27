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

        RuleFor(x => x.ArtistTypeSlug).NotEmpty().MustAsync(BeArtistTypeSlugExistingInDbAsync);
        RuleFor(x => x.Name).NotEmpty().MustAsync(BeUniqueNameAsync);
    }

    private async Task<bool> BeUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        var artist = await _dbContext.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == name, cancellationToken);

        return artist is null;
    }

    private async Task<bool> BeArtistTypeSlugExistingInDbAsync(
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