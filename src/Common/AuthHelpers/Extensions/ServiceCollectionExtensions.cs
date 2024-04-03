using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Musdis.AuthHelpers.Options;

namespace Musdis.AuthHelpers.Extensions;

/// <summary>
///     Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds common authentication.
    /// </summary>
    /// 
    /// <param name="services">
    ///     The <see cref="IServiceCollection"/> instance.
    /// </param>
    /// <param name="configuration">
    ///     The <see cref="IConfiguration"/> instance.
    /// </param>
    /// <param name="jwtSectionName">
    ///     The name of the JWT configuration section.
    /// </param>
    /// 
    /// <returns>
    ///     The <see cref="IServiceCollection"/> instance with added authentication.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="JwtOptions"/> configuration section is missing.
    /// </exception>
    public static IServiceCollection AddCommonAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        string jwtSectionName
    )
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                    .GetSection(jwtSectionName)
                    .Get<JwtOptions>()
                    ?? throw new InvalidOperationException("A JwtOptions configuration section is missing.");

                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key)
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}