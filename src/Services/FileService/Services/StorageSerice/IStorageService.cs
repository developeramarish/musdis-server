using Results;

namespace FileService.Services.StorageService;

public interface IStorageService
{
    Task<Result<Uri>> UploadFileAsync(
        string fileName,
        IFormFile file,
        CancellationToken cancellationToken = default
    );

    Task<Result<Uri>> GetFileUriAsync(
        string fileName, 
        CancellationToken cancellationToken = default
    );
}