namespace Musdis.FileService.Requests;

public sealed record UploadUserAvatarRequest(
    string UserId,
    IFormFile File
);