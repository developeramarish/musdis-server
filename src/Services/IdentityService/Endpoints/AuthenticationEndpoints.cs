using Musdis.IdentityService.Models.Requests;
using Musdis.IdentityService.Services.AuthenticationService;

using Microsoft.AspNetCore.Mvc;

namespace Musdis.IdentityService.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthentication(
        this RouteGroupBuilder group
    )
    {
        group.MapPost("/sign-in", async (
            [FromBody] SignInRequest request,
            [FromServices] IAuthenticationService authenticationService
        ) =>
        {
            var result = await authenticationService.SignInAsync(request);
        });


        return group;
    }
}

