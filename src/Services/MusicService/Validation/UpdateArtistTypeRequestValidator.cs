using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="UpdateArtistTypeRequest"/>.
/// </summary>
public class UpdateArtistTypeRequestValidator : AbstractValidator<UpdateArtistTypeRequest>
{
    private readonly MusicServiceDbContext _dbContext;

    public UpdateArtistTypeRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        
        RuleFor(x => x.Name).NotEmpty();
        
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithErrorCode(ErrorCodes.NonUniqueData)
            .WithMessage("Artist type name must be unique.");
    }

    private async Task<bool> BeUniqueNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var artistType = await _dbContext.ArtistTypes.FirstOrDefaultAsync(
            a => a.Name == name,
            cancellationToken
        );

        return artistType is null;
    }
}
