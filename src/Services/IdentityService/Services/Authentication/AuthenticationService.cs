using FluentValidation;

using Musdis.IdentityService.Extensions;
using Musdis.IdentityService.Models;
using Musdis.IdentityService.Dtos;
using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Services.Jwt;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Musdis.OperationResults;
using Musdis.ResponseHelpers.Errors;
using Musdis.OperationResults.Extensions;

namespace Musdis.IdentityService.Services.Authentication;

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
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var user = request.ToUser();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new InternalServerError(
                "Could not create user, try again later!"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var signInResult = await GetAuthenticatedUserDtoAsync(user, request.Password, cancellationToken);

        return signInResult;
    }

    /// <summary>
    ///     Gets the user's authentication info.
    /// </summary>
    /// 
    /// <param name="user">
    ///     The user to get info of.
    /// </param>
    /// <param name="password">
    ///     The password that came from request.
    /// </param>
    /// <param name="cancellationToken">
    ///     The token to cancel the operation.
    /// </param>
    /// 
    /// <returns>
    ///     User's info, JWT and their claims wrapped in <see cref="AuthenticatedUserDto"/>.
    /// </returns>
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
            return new InternalServerError(
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