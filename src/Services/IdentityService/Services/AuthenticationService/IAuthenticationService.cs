using IdentityService.Models.Dtos;
using IdentityService.Models.Requests;

using Results;

namespace IdentityService.Services.AuthenticationService;

/// <summary>
/// Represents an authentication service that provides user sign-in and sign-up functionality.
/// </summary>
public interface IAuthenticationService
{

    /// <summary>
    /// Attempts to sign in a user with the provided credentials.
    /// </summary>
    /// <param name="request">The sign-in request containing user credentials.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation, which returns 
    ///     a <see cref="Result{AuthenticatedUserDto}"/> containing the outcome of the sign-in attempt.
    /// </returns>
    Task<Result<AuthenticatedUserDto>> SignInAsync(
        SignInRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Attempts to sign up a new user with the provided user information.
    /// </summary>
    /// <param name="request">The sign-up request containing user information.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation, which returns 
    ///     a <see cref="Result{AuthenticatedUserDto}"/> containing the outcome of the sign-up attempt.
    /// </returns>
    Task<Result<AuthenticatedUserDto>> SignUpAsync(
        SignUpRequest request,
        CancellationToken cancellationToken = default
    );
}