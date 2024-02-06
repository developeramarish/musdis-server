using FluentValidation;

using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateArtistTypeRequest"/>.
/// </summary>
public class CreateArtistTypeRequestValidator : AbstractValidator<CreateArtistTypeRequest>
{
    public CreateArtistTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
