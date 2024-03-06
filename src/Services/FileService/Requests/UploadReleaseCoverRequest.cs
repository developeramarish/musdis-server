namespace Musdis.FileService.Requests;

public sealed record UploadReleaseCoverRequest(
    Guid ReleaseId,
    IFormFile File
);