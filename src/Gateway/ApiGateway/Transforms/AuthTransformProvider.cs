using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;

using Musdis.ApiGateway.Defaults;
using Musdis.ApiGateway.Responses;

using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Musdis.ApiGateway.Transforms;


public class AuthTransformProvider : ITransformProvider
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IExceptionHandler _exceptionHandler;
    public AuthTransformProvider(IExceptionHandler exceptionHandler)
    {
        _exceptionHandler = exceptionHandler;
    }
    public void ValidateRoute(TransformRouteValidationContext context) { }

    public void ValidateCluster(TransformClusterValidationContext context) { }

    public void Apply(TransformBuilderContext context)
    {
        context.AddResponseTransform(async responseContext =>
        {
            if (responseContext.ProxyResponse is null || (
                responseContext.HttpContext.Request.Path != "/identity-service/sign-in" &&
                responseContext.HttpContext.Request.Path != "/identity-service/sign-up"
            ))
            {
                return;
            }

            try
            {
                if (responseContext.HttpContext.Response.StatusCode == 401)
                {
                    responseContext.HttpContext.Response.Cookies.Delete(CookieNames.Jwt);
                }
                if (responseContext.HttpContext.Response.StatusCode != 200)
                {
                    return;
                }

                responseContext.SuppressResponseBody = true;
                var stream = await responseContext.ProxyResponse.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var body = await reader.ReadToEndAsync();

                var authenticatedUser = JsonSerializer.Deserialize<AuthenticatedUser>(body, _jsonSerializerOptions)
                    ?? throw new InvalidOperationException("Cannot deserialize authenticated user");

                if (string.IsNullOrEmpty(body))
                {
                    throw new InvalidOperationException("Cannot authenticate user, empty response body");
                }

                if (string.IsNullOrEmpty(authenticatedUser.Jwt))
                {
                    throw new InvalidOperationException("Cannot authenticate user, empty JWT");
                }

                responseContext.HttpContext.Response.Cookies.Append(
                    CookieNames.Jwt,
                    authenticatedUser.Jwt,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        MaxAge = TimeSpan.FromDays(7)
                    }
                );

                var user = authenticatedUser.User;
                var userBody = JsonSerializer.Serialize(user, _jsonSerializerOptions);
                var bytes = Encoding.UTF8.GetBytes(userBody);

                responseContext.HttpContext.Response.ContentLength = bytes.Length;
                await responseContext.HttpContext.Response.Body.WriteAsync(bytes);

            }
            catch (Exception ex)
            {
                await _exceptionHandler.TryHandleAsync(responseContext.HttpContext, ex, default);
            }
        });
    }
}