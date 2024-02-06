using FluentValidation;

using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="CreateReleaseTypeRequest"/>.
/// </summary>
public class CreateReleaseTypeRequestValidator : AbstractValidator<CreateReleaseTypeRequest>
{
    public CreateReleaseTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}