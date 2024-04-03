using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Musdis.FileService.Data;
using Musdis.FileService.Dtos;
using Musdis.FileService.Models;
using Musdis.FileService.Utils;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.FileService.Services.Storage;

using FilesMetadataResult = Result<IList<FileMetadataDto>>;

/// <inheritdoc cref="IStorageService"/>
public class StorageService : IStorageService
{
    private readonly FileServiceDbContext _dbContext;
    private readonly IStorageProvider _storageProvider;
    private readonly IValidator<IFormFile> _formFileValidator;

    public StorageService(
        IStorageProvider storageProvider,
        FileServiceDbContext dbContext,
        IValidator<IFormFile> formFileValidator
    )
    {
        _storageProvider = storageProvider;
        _dbContext = dbContext;
        _formFileValidator = formFileValidator;
    }

    public async Task<Result<FileMetadataDto>> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _formFileValidator.ValidateAsync(file, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationError(
                "Cannot upload file, validation failed",
                validationResult.Errors.Select(err => err.ErrorMessage)
            ).ToValueResult<FileMetadataDto>();
        }

        var fileId = Guid.NewGuid();

        var extension = Path.GetExtension(file.FileName);
        var fileTypeResult = FileHelper.GetFileType(extension);
        if (fileTypeResult.IsFailure)
        {
            return fileTypeResult.Error.ToValueResult<FileMetadataDto>();
        }

        var newName = Path.Combine(fileId.ToString(), extension);
        var filePathResult = GenerateFilePath(newName);
        if (filePathResult.IsFailure)
        {
            return filePathResult.Error.ToValueResult<FileMetadataDto>();
        }

        var uploadResult = await _storageProvider.UploadFileAsync(filePathResult.Value, file, cancellationToken);
        if (uploadResult.IsFailure)
        {
            return uploadResult.Error.ToValueResult<FileMetadataDto>();
        }

        var fileMetadata = new FileMetadata
        {
            Id = fileId,
            Url = uploadResult.Value.ToString(),
            Path = filePathResult.Value,
            FileType = fileTypeResult.Value
        };

        await _dbContext.FilesMetadata.AddAsync(fileMetadata, cancellationToken);

        return FileMetadataDto.FromFileMetadata(fileMetadata).ToValueResult();
    }

    public async Task<FilesMetadataResult> UploadFilesAsync(
        IList<IFormFile> files,
        CancellationToken cancellationToken = default
    )
    {
        foreach (var file in files)
        {
            var result = await _formFileValidator.ValidateAsync(file, cancellationToken);
            if (!result.IsValid)
            {
                return new ValidationError(
                    "Cannot upload file, validation failed",
                    result.Errors.Select(err => err.ErrorMessage)
                ).ToValueResult<IList<FileMetadataDto>>();
            }
        }

        var filesMetadata = new List<FileMetadataDto>(files.Count);
        foreach (var file in files)
        {
            var uploadResult = await UploadFileAsync(file, cancellationToken);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error.ToValueResult<IList<FileMetadataDto>>();
            }

            filesMetadata.Add(uploadResult.Value);
        }
        // Cannot implicitly convert to `IList<FileMetadataDto>` from `List<FileMetadataDto>`.
        IList<FileMetadataDto> metadataDtos = filesMetadata;

        return metadataDtos.ToValueResult();
    }

    public async Task<Result> DeleteFileAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var fileMetadata = await _dbContext.FilesMetadata
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        if (fileMetadata is null)
        {
            return new NoContentError().ToResult();
        }

        var deleteResult = await _storageProvider.DeleteFileAsync(fileMetadata.Path, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToResult();
        }

        _dbContext.FilesMetadata.Remove(fileMetadata);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<FileMetadataDto>> GetFileMetadataAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var fileMetadata = await _dbContext.FilesMetadata
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        if (fileMetadata is null)
        {
            return new NotFoundError(
                $"Cannot find file with {{Id}} == {id}"
            ).ToValueResult<FileMetadataDto>();
        }

        return FileMetadataDto.FromFileMetadata(fileMetadata).ToValueResult();
    }

    public async Task<FilesMetadataResult> GetFilesMetadataAsync(
        IList<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        var filesMetadata = await _dbContext.FilesMetadata
            .Where(f => ids.Contains(f.Id))
            .ToArrayAsync(cancellationToken);

        if (filesMetadata.Length == 0)
        {
            return new NotFoundError(
                "Cannot find files with specified ids"
            ).ToValueResult<IList<FileMetadataDto>>();
        }

        IList<FileMetadataDto> result = FileMetadataDto.FromFilesMetadata(filesMetadata).ToList();

        return result.ToValueResult();
    }

    public async Task<Result> SaveChangesToDbAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Cannot save changes to the database: {ex.Message}");
        }
    }

    private static Result<string> GenerateFilePath(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (extension is null || !FileHelper.IsExtensionSupported(extension))
        {
            return Result<string>.Failure(
                "Cannot generate file path for this file: extension missing or not supported."
            );
        }

        var fileTypeResult = FileHelper.GetFileType(extension);
        if (fileTypeResult.IsFailure)
        {
            return fileTypeResult.Error.ToValueResult<string>();
        }

        try
        {
            var path = Path.Combine(fileTypeResult.Value, fileName);

            return path.ToValueResult();
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Cannot generate file path: {ex.Message}");
        }
    }
}