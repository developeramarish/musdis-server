using FluentValidation;

using Musdis.MusicService.Requests;

namespace Musdis.MusicService.Validation;

/// <summary>
///     The validator for <see cref="UpdateReleaseTypeRequest"/>.
/// </summary>
public class UpdateReleaseTypeRequestValidator : AbstractValidator<UpdateReleaseTypeRequest>
{
    public UpdateReleaseTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}