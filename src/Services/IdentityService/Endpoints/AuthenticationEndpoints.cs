using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Services.Authentication;
using Musdis.IdentityService.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace Musdis.IdentityService.Endpoints;

/// <summary>
/// Endpoints for authentication.
/// </summary>
public static class AuthenticationEndpoints
{
    /// <summary>
    /// Maps authentication endpoints.
    /// </summary>
    /// <param name="group">Route group.</param>
    /// <returns>Route group with mapped authentication.</returns>
    public static RouteGroupBuilder MapAuthentication(
        this RouteGroupBuilder group
    )
    {
        group.MapPost("/sign-in", async (
            [FromBody] SignInRequest request,
            [FromServices] IAuthenticationService authenticationService,
            HttpContext context,
            CancellationToken cancellationToken
        ) =>
        {
            var result = await authenticationService.SignInAsync(request, cancellationToken);

            return result switch
            {
                { IsSuccess: true } => Results.Ok(result.Value),
                { IsSuccess: false } => result.Error.ToProblemResult(context.Request.Path),
            };

        });

        group.MapPost("/sign-up", async (
            [FromBody] SignUpRequest request,
            [FromServices] IAuthenticationService authenticationService,
            HttpContext context,
            CancellationToken cancellationToken
        ) =>
        {
            var result = await authenticationService.SignUpAsync(request, cancellationToken);

            return result switch
            {
                { IsSuccess: true } => Results.Ok(result.Value),
                { IsSuccess: false } => result.Error.ToProblemResult(context.Request.Path),
            };
        });
        
        return group;
    }
}

