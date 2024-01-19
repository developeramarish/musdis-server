using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Musdis.IdentityService.Controllers;

/// <summary>
/// Error handling controller.
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class ErrorController : ControllerBase
{
    /// <summary>
    /// Handles errors in publish. Hides all details.
    /// </summary>
    /// <returns></returns>
    [Route("error")]
    [AllowAnonymous]
    public IActionResult HandleError()
    {
        return Problem();
    }

    /// <summary>
    /// Handles errors in development. Shows all details.
    /// </summary>
    [Route("error-development")]
    public IActionResult HandleErrorDevelopment()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        
        if (exceptionHandlerFeature is null)
        {
            return Problem();
        }

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message
        );
    }
}