using FluentValidation;

using Musdis.IdentityService.Models;
using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Options;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Musdis.IdentityService.Validation;

/// <summary>
///     Validates <see cref="SignUpRequest"/>s to ensure they meet certain criteria.
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    private readonly IdentityConfigOptions _identityOptions;
    private readonly UserManager<User> _userManager;
    public SignUpRequestValidator(
        UserManager<User> userManager,
        IOptions<IdentityConfigOptions> options
    )
    {
        _userManager = userManager;
        _identityOptions = options.Value;

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("Username must be unique.")
            .WithErrorCode(ErrorCodes.NonUniqueData);

        RuleFor(x => x.UserName)
            .Must(BeCorrectUserName)
            .WithMessage(_ => GenerateUserNameErrorMessage());

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(BeUniqueEmailAsync)
            .WithMessage("Email must be unique.")
            .WithErrorCode(ErrorCodes.NonUniqueData);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Must(BeCorrectPassword);
    }

    private string GenerateUserNameErrorMessage()
    {
        return $"""
            Username must be at least ${_identityOptions.User.MinUserNameLength} characters long and not longer than ${_identityOptions.User.MaxUserNameLength} characters long.
            Allowed characters: {_identityOptions.User.AllowedUserNameCharacters}
        """;
    }

    private async Task<bool> BeUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(name);

        return user is null;
    }
    private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user is null;
    }

    private bool BeCorrectUserName(string userName)
    {
        if (userName.Length < _identityOptions.User.MinUserNameLength)
        {
            return false;
        }
        if (userName.Length > _identityOptions.User.MaxUserNameLength)
        {
            return false;
        }
        if (!userName.All(_identityOptions.User.AllowedUserNameCharacters.Contains))
        {
            return false;
        }

        return true;
    }
    private bool BeCorrectPassword(string password)
    {
        if (_identityOptions.Password.RequireDigit && !password.Any(char.IsDigit))
        {
            return false;
        }
        if (_identityOptions.Password.RequireLowercase && !password.Any(char.IsLower))
        {
            return false;
        }
        if (password.Length < _identityOptions.Password.RequiredLength)
        {
            return false;
        }
        if (_identityOptions.Password.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
        {
            return false;
        }
        if (new string(password.Distinct().ToArray()).Length < _identityOptions.Password.RequiredUniqueChars)
        {
            return false;
        }

        return true;
    }
}