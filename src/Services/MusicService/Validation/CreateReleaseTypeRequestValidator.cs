using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateReleaseTypeRequest"/>.
/// </summary>
public class CreateReleaseTypeRequestValidator : AbstractValidator<CreateReleaseTypeRequest>
{
    private readonly MusicServiceDbContext _dbContext;
    public CreateReleaseTypeRequestValidator(MusicServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithErrorCode(ErrorCodes.NonUniqueData)
            .WithMessage("Release type name must be unique.");
    }

    private async Task<bool> BeUniqueNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var releaseType = await _dbContext.ReleaseTypes.FirstOrDefaultAsync(
            a => a.Name == name,
            cancellationToken
        );

        return releaseType is null;
    }
}