using FluentValidation;

using Musdis.IdentityService.Models;
using Musdis.IdentityService.Models.Requests;
using Musdis.IdentityService.Options;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Musdis.IdentityService.Validation;

/// <summary>
/// Validates <see cref="SignUpRequest"/>s to ensure they meet certain criteria.
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    private readonly IdentityPasswordOptions _passwordOptions;
    private readonly UserManager<User> _userManager;
    public SignUpRequestValidator(
        UserManager<User> userManager,
        IOptions<IdentityPasswordOptions> options
    )
    {
        _userManager = userManager;
        _passwordOptions = options.Value;

        RuleFor(x => x.Password).NotEmpty().Must(BeCorrectPassword);
        RuleFor(x => x.UserName).NotEmpty().MustAsync(BeUniqueNameAsync);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MustAsync(BeUniqueEmailAsync);
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
    private bool BeCorrectPassword(string password)
    {
        if (_passwordOptions.RequireDigit && !password.Any(char.IsDigit))
        {
            return false;
        }
        if (_passwordOptions.RequireLowercase && !password.Any(char.IsLower))
        {
            return false;
        }
        if (password.Length < _passwordOptions.RequiredLength)
        {
            return false;
        }
        if (_passwordOptions.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
        {
            return false;
        }
        if (new string(password.Distinct().ToArray()).Length < _passwordOptions.RequiredUniqueChars)
        {
            return false;
        }

        return true;
    }
}