using IdentityService.Errors;
using IdentityService.Models.Dtos;
using IdentityService.Models.Requests;
using IdentityService.Services.AuthenticationService;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

/// <summary>
/// Controller responsible for user authentication.
/// </summary>
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Sign user in.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>
    /// An HTTP response with the user object and JWT if sign-in is successful; 
    /// otherwise, returns an appropriate HTTP status code and error details.
    /// </returns>    
    [Route("sign-in")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedUserDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInAsync(SignInRequest request)
    {
        var result = await _authenticationService.SignInAsync(request);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error switch
        {
            UnauthorizedError => Unauthorized(),
            ValidationError => BadRequest(result.Error.Description),
            InternalError => Problem(result.Error.Description),
            _ => Problem("Internal error!")
        };
    }

    /// <summary>
    /// Sign up a new user.
    /// </summary>
    /// <param name="request">Sign up request.</param>
    /// <returns>
    /// An HTTP response with the user object and JWT if sign-up is successful; 
    /// otherwise, returns an appropriate HTTP status code and error details.
    /// </returns>
    [Route("sign-up")]
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticatedUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUpAsync(SignUpRequest request)
    {
        var result = await _authenticationService.SignUpAsync(request);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error switch
        {
            UnauthorizedError => Unauthorized(),
            ValidationError => BadRequest(result.Error.Description),
            InternalError => Problem(result.Error.Description),
            _ => Problem("Internal error!")
        };
    }

    [Route("test")]
    [Authorize]
    [HttpGet]
    public IActionResult TestAuthorization()
    {
        return Ok("successfully authorized");
    }
}