using FluentValidation;

using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="UpdateArtistTypeRequest"/>.
/// </summary>
public class UpdateArtistTypeRequestValidator : AbstractValidator<UpdateArtistTypeRequest>
{
    public UpdateArtistTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
