using Microsoft.AspNetCore.Mvc;

using Musdis.FileService.Services.Storage;
using Musdis.ResponseHelpers.Extensions;
using Musdis.ResponseHelpers.Responses;

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
    public static RouteGroupBuilder MapFiles(
        this RouteGroupBuilder groupBuilder
    )
    {
        groupBuilder.MapPost("/", HandlePostFileAsync);

        groupBuilder.MapGet("/", HandleGetFileUrlsAsync);

        return groupBuilder;
    }

    public static async Task<IResult> HandlePostFileAsync(
        [FromForm] IFormFile file,
        [FromServices] IStorageService storageService,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var result = await storageService.UploadFileAsync(file, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToHttpResult(context.Request.Path);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> HandleGetFileUrlsAsync(
        [FromQuery] string[] filePaths,
        [FromServices] IStorageProvider storageProvider,
        HttpContext context,
        CancellationToken cancellationToken
    )
    {
        var result = await storageProvider.GetFileUrlsAsync(filePaths, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToHttpResult(context.Request.Path);
        }

        return Results.Ok(new DataResponse<IList<Uri>>(result.Value));
    }

    // public static async Task<IResult> HandleGetFileUrlAsync(
    //     [FromQuery] string filePath,
    //     [FromServices] IStorageProvider storageProvider,
    //     HttpContext context,
    //     CancellationToken cancellationToken
    // )
    // {
    //     var result = await storageProvider.GetFileUrlAsync(filePath, cancellationToken);
    //     if (result.IsFailure)
    //     {
    //         return result.Error.ToHttpResult(context.Request.Path);
    //     }

    //     return Results.Ok(new DataResponse<Uri>(result.Value));
    // }
}