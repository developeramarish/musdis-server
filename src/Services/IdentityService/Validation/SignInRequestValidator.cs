using FluentValidation;

using IdentityService.Models.Requests;

namespace IdentityService.Validation;

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