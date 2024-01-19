using Musdis.FileService.Services.StorageService;

using Microsoft.AspNetCore.Mvc;

namespace Musdis.FileService.Controllers;

[Route("api/file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IStorageService _storageService;

    public FileController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [Route("upload")]
    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(
        IFormFile formFile
    )
    {
        var result = await _storageService.UploadFileAsync("newFile", formFile);

        return result switch
        {
            { IsFailure: true } => NotFound(result.Error.Description),
            { IsFailure: false } => Ok(result.Value.ToString())
        };
    }

    [Route("get-file-uri")]
    [HttpGet]
    public async Task<IActionResult> GetFileUriAsync()
    {
        var result = await _storageService.GetFileUriAsync("images/121.jpg");

        return result switch
        {
            { IsFailure: true } => NotFound(result.Error.Description),
            { IsFailure: false } => Ok(result.Value.ToString())
        };
    }
}