using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Musdis.AuthHelpers.Extensions;

/// <summary>
///     Extension methods for <see cref="SwaggerGenOptions"/> class.
/// </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    ///     Adds common authorization to swagger.
    /// </summary>
    /// 
    /// <param name="options">
    ///     The <see cref="SwaggerGenOptions"/> instance.
    /// </param>
    public static void AddJwtAuthorization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter your JWT token.",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}