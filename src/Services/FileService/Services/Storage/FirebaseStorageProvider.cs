using Musdis.FileService.Options;

using Google.Cloud.Storage.V1;

using Microsoft.Extensions.Options;

using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.FileService.Services.Storage;

public class FirebaseStorageProvider : IStorageProvider
{
    private readonly StorageClient _storageClient;
    private readonly FirebaseOptions _firebaseOptions;

    public FirebaseStorageProvider(
        StorageClient storageClient,
        IOptions<FirebaseOptions> firebaseOptions
    )
    {
        _storageClient = storageClient;
        _firebaseOptions = firebaseOptions.Value;
    }

    public async Task<Result<Uri>> GetFileUrlAsync(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var objects = _storageClient.ListObjectsAsync(_firebaseOptions.DefaultBucketName, filePath);
            var entriesCount = await objects.CountAsync(cancellationToken);

            if (entriesCount == 0)
            {
                return new NotFoundError($"Files with name {{{filePath}}} not found")
                    .ToValueResult<Uri>();
            }
            var file = await _storageClient.GetObjectAsync(
                _firebaseOptions.DefaultBucketName,
                filePath,
                cancellationToken: cancellationToken
            );

            return GenerateUri(file);
        }
        catch (Exception ex)
        {
            return new Error(
                $"Cannot get file uri: {ex.Message}"
            ).ToValueResult<Uri>();
        }
    }

    public async Task<Result<List<Uri>>> GetFileUrlsAsync(
        IList<string> filePaths,
        CancellationToken cancellationToken = default
    )
    {
        var urls = new List<Uri>(filePaths.Count);
        foreach (var fileName in filePaths)
        {
            var result = await GetFileUrlAsync(fileName, cancellationToken);
            if (result.IsFailure)
            {
                return Result<List<Uri>>.Failure(
                    $"Cannot get file urls, error occurred with one of the file names: {result.Error.Description}"
                );
            }

            urls.Add(result.Value);
        }

        return urls.ToValueResult();
    }

    public async Task<Result<Uri>> UploadFileAsync(
        string filePath,
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            
            var uploaded = await _storageClient.UploadObjectAsync(
                _firebaseOptions.DefaultBucketName,
                filePath,
                file.ContentType,
                stream
            );

            return GenerateUri(uploaded);
        }
        catch (Exception ex)
        {
            return new Error(
                $"Cannot upload file: {ex.Message}"
            ).ToValueResult<Uri>();
        }
    }

    public async Task<Result> DeleteFileAsync(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(
                _firebaseOptions.DefaultBucketName,
                filePath,
                cancellationToken: cancellationToken
            );

            await _storageClient.DeleteObjectAsync(obj, cancellationToken: cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Cannot delete file: {ex.Message}");
        }
    }

    private static Result<Uri> GenerateUri(Google.Apis.Storage.v1.Data.Object file)
    {
        if (file is null)
        {
            return new Error(
                "Cannot generate Uri. File is null."
            ).ToValueResult<Uri>();
        }

        return new Uri(
            $"https://storage.googleapis.com/{file.Bucket}/{file.Name}"
        ).ToValueResult();
    }
}