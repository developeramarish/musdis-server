using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Musdis.ResponseHelpers.Errors;

namespace Musdis.FileService.Services.Exceptions;

/// <summary>
///     The application exception handler for development.
/// </summary>
/// <remarks>
///     Provides an additional information about exception that was thrown.
/// </remarks>
public class DeveloperExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DeveloperExceptionHandler> _logger;

    public DeveloperExceptionHandler(ILogger<DeveloperExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Exception occurred. Details: {Error}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error",
            Instance = httpContext.Request.Path,
            Type = InternalServerError.ProblemDetailsType,
            Detail = exception.Message,
            Extensions = 
            new Dictionary<string, object?>
            {
                { "exception", exception }
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}