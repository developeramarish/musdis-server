using FluentValidation;

using Musdis.IdentityService.Errors;
using Musdis.IdentityService.Extensions;
using Musdis.IdentityService.Models;
using Musdis.IdentityService.Models.Dtos;
using Musdis.IdentityService.Models.Requests;
using Musdis.IdentityService.Services.JwtGenerator;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.IdentityService.Models.Entities;

namespace Musdis.IdentityService.Services.AuthenticationService;

/// <inheritdoc cref="IAuthenticationService"/>
public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IValidator<SignUpRequest> _signUpValidator;

    public AuthenticationService(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IJwtGenerator jwtGenerator,
        IValidator<SignUpRequest> signUpValidator
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _signUpValidator = signUpValidator;
    }

    /// <summary>
    /// Attempts to sign in a user with the provided credentials.
    /// </summary>
    /// <param name="request">The sign-in request containing user credentials.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Result{AuthenticatedUserDto}"/> representing the outcome of the sign-in attempt.</returns>
    public async Task<Result<AuthenticatedUserDto>> SignInAsync(
        SignInRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
            ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);
        if (user is null)
        {
            return new UnauthorizedError().ToValueResult<AuthenticatedUserDto>();
        }
        var result = await GetAuthenticatedUserDtoAsync(user, request.Password, cancellationToken);

        return result;
    }

    /// <summary>
    /// Attempts to sign up a new user with the provided user information.
    /// </summary>
    /// <param name="request">The sign-up request containing user information.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Result{AuthenticatedUserDto}"/> representing the outcome of the sign-up attempt.</returns>
    public async Task<Result<AuthenticatedUserDto>> SignUpAsync(
        SignUpRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _signUpValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Cannot sign user up, incorrect data",
                validationResult.Errors
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var user = request.ToUser();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new InternalError(
                "Could not create user, try again later!"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var signInResult = await GetAuthenticatedUserDtoAsync(user, request.Password, cancellationToken);

        return signInResult;
    }

    private async Task<Result<AuthenticatedUserDto>> GetAuthenticatedUserDtoAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default
    )
    {

        if (user is null || password.IsNullOrEmpty())
        {
            return new UnauthorizedError()
                .ToValueResult<AuthenticatedUserDto>();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
        {
            return new UnauthorizedError().ToValueResult<AuthenticatedUserDto>();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var token = _jwtGenerator.GenerateToken(new GenerateJwtRequest(
            new UserReadDto(
                user.Id,
                user.UserName!,
                user.Email!
            ),
            claims
        ));

        if (cancellationToken.IsCancellationRequested)
        {
            return new InternalError(
                "Request cancelled"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        return new AuthenticatedUserDto(
            user.Id,
            user.UserName!,
            user.Email!,
            token,
            claims.ToKeyValuePairs()
        ).ToValueResult();
    }
}