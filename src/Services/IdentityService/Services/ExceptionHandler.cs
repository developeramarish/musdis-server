using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Musdis.IdentityService.Defaults;

namespace Musdis.IdentityService.Services;

/// <summary>
/// Global application exception handler.
/// </summary>
public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Exception occurred. Details: {Error}", exception.Message);


        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error",
            Instance = httpContext.Request.Path,
            Type = ProblemDetailsErrorTypes.InternalError,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;

    }
}