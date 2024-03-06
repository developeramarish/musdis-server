using Microsoft.AspNetCore.Mvc;

using Musdis.FileService.Services.Storage;
using Musdis.ResponseHelpers.Extensions;

namespace Musdis.FileService.Endpoints;

/// <summary>
///     Endpoints for file management.
/// </summary>
public static class FileEndpoints
{
    /// <summary>
    ///     Maps file management endpoints.
    /// </summary>
    /// 
    /// <param name="groupBuilder">
    ///     The route group to be mapped.
    /// </param>
    /// 
    /// <returns>
    ///     Route group with mapped endpoints.
    /// </returns>
    public static RouteGroupBuilder MapSomething(
        this RouteGroupBuilder groupBuilder
    )
    {
        // TODO add endpoints
    
        return groupBuilder;
    }
}