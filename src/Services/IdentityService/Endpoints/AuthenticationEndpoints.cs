using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Services.Authentication;

using Microsoft.AspNetCore.Mvc;
using Musdis.ResponseHelpers.Errors;
using Musdis.ResponseHelpers.Extensions;
using Musdis.IdentityService.Authorization;

namespace Musdis.IdentityService.Endpoints;

/// <summary>
///     Endpoints for authentication.
/// </summary>
public static class AuthenticationEndpoints
{
    /// <summary>
    ///     Maps authentication endpoints.
    /// </summary>
    /// 
    /// <param name="groupBuilder">
    ///     The route group to be mapped.
    /// </param>
    /// 
    /// <returns>
    ///     Route group with mapped authentication.
    /// </returns>
    public static RouteGroupBuilder MapAuthentication(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapPost("/sign-in", HandleSignInAsync);

        groupBuilder.MapPost("/sign-up", HandleSignUpAsync);

        groupBuilder.MapPost("/sign-admin-up", HandleSignAdminUpAsync)
            .RequireAuthorization(AuthorizationPolicies.Admin);

        return groupBuilder;
    }

    private static async Task<IResult> HandleSignInAsync(
        [FromBody] SignInRequest request,
        [FromServices] IAuthenticationService authenticationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var result = await authenticationService.SignInAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.ToHttpResult(context.Request.Path);
    }

    private static async Task<IResult> HandleSignUpAsync(
        [FromBody] SignUpRequest request,
        [FromServices] IAuthenticationService authenticationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var result = await authenticationService.SignUpAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.ToHttpResult(context.Request.Path);
    }

    private static async Task<IResult> HandleSignAdminUpAsync(
        [FromBody] SignUpRequest request,
        [FromServices] IAuthenticationService authenticationService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var result = await authenticationService.SignAdminUpAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.ToHttpResult(context.Request.Path);
    }
}

