using FileService.Errors;
using FileService.Options;

using Google.Cloud.Storage.V1;

using Microsoft.Extensions.Options;

using Results;
using Results.Extensions;

namespace FileService.Services.StorageService;

public class FirebaseStorageService : IStorageService
{
    private readonly StorageClient _storageClient;
    private readonly FirebaseOptions _firebaseOptions;

    public FirebaseStorageService(
        StorageClient storageClient,
        IOptions<FirebaseOptions> firebaseOptions
    )
    {
        _storageClient = storageClient;
        _firebaseOptions = firebaseOptions.Value;
    }

    public async Task<Result<Uri>> GetFileUriAsync(
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        var objects = _storageClient.ListObjectsAsync(_firebaseOptions.DefaultBucketName, fileName);
        var entriesCount = await objects.CountAsync(cancellationToken);

        if (entriesCount == 0)
        {
            return new NotFoundError($"Files with name {fileName} not found")
                .ToValueResult<Uri>();
        }

        var file = await _storageClient.GetObjectAsync(
            _firebaseOptions.DefaultBucketName,
            fileName,
            null,
            cancellationToken
        );

        return new Uri($"{file.MediaLink}&token={file.Metadata["firebaseStorageDownloadTokens"]}")
            .ToResult();
    }

    public async Task<Result<Uri>> UploadFileAsync(
        string fileName,
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);

        var uploaded = await _storageClient.UploadObjectAsync(
            _firebaseOptions.DefaultBucketName,
            fileName,
            file.ContentType,
            stream
        );

        return new Uri($"{uploaded.SelfLink}")
            .ToResult();
    }
}