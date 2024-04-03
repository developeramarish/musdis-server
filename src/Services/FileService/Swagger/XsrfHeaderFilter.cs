using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Musdis.FileService.Swagger;

/// <summary>
/// Operation filter to add the requirement of the custom header
/// </summary>
public class XsrfHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-XSRF-TOKEN",
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema { Type = "String" },
            Required = false 
        });
    }
}