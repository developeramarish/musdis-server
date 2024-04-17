using System.Security.Claims;

using FluentValidation;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Musdis.IdentityService.Models;
using Musdis.IdentityService.Dtos;
using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Services.Jwt;
using Musdis.IdentityService.Validation;


using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;
using Musdis.AuthHelpers.Authorization;
using MassTransit;
using Musdis.MessageBrokerHelpers.Events;

namespace Musdis.IdentityService.Services.Authentication;

/// <inheritdoc cref="IAuthenticationService"/>
public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IValidator<SignUpRequest> _signUpValidator;

    public AuthenticationService(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IJwtGenerator jwtGenerator,
        IValidator<SignUpRequest> signUpValidator,
        IPublishEndpoint publishEndpoint
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _signUpValidator = signUpValidator;
        _publishEndpoint = publishEndpoint;
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
            if (validationResult.Errors.Exists(e => e.ErrorCode == ErrorCodes.NonUniqueData))
            {
                var errorMessages = validationResult.Errors
                    .Where(f => f.ErrorCode == ErrorCodes.NonUniqueData)
                    .Select(f => f.ErrorMessage);
                var message = string.Join("\n", errorMessages);

                return new ConflictError(
                    $"Some data is not unique: {message}"
                ).ToValueResult<AuthenticatedUserDto>();
            }

            return new ValidationError(
                "Cannot sign user up, incorrect data.",
                validationResult.Errors.Select(f => f.ErrorMessage)
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var user = request.ToUser();

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {

            return new InternalServerError(
                "Could not create user, try again later!"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        await _publishEndpoint.Publish(new FileUsed(request.AvatarFile.Id));

        var signInResult = await GetAuthenticatedUserDtoAsync(user, request.Password, cancellationToken);

        return signInResult;
    }

    public async Task<Result<AuthenticatedUserDto>> SignAdminUpAsync(
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

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return new InternalServerError(
                "Could not create user, try again later!"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var claimAddResult = await _userManager.AddClaimAsync(
            user,
            new(ClaimDefaults.Admin, "true", ClaimValueTypes.Boolean)
        );
        if (!claimAddResult.Succeeded)
        {
            return new InternalServerError(
                "Could not add claim to user, try again later!"
            ).ToValueResult<AuthenticatedUserDto>();
        }

        await _publishEndpoint.Publish(new FileUsed(request.AvatarFile.Id));

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
            return new UnauthorizedError(
                "User unauthorized: invalid credentials."
            ).ToValueResult<AuthenticatedUserDto>();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var tokenResult = _jwtGenerator.GenerateToken(new GenerateJwtRequest(
            new UserReadDto(
                user.Id,
                user.UserName!,
                user.Email!
            ),
            claims
        ));

        if (tokenResult.IsFailure)
        {
            return tokenResult.Error.ToValueResult<AuthenticatedUserDto>();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return new InternalServerError(
                "Request cancelled."
            ).ToValueResult<AuthenticatedUserDto>();
        }

        return new AuthenticatedUserDto(
            user.Id,
            user.UserName!,
            user.Email!,
            tokenResult.Value,
            claims.ToDictionary(c => c.Type, c => c.Value)
        ).ToValueResult();
    }
}