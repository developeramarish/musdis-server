using FluentValidation;

using Musdis.IdentityService.Models.Requests;

namespace Musdis.IdentityService.Validation;

/// <summary>
/// Validates <see cref="SignInRequest"/>s to ensure they meet certain criteria.
/// </summary>
public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.UserNameOrEmail).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}