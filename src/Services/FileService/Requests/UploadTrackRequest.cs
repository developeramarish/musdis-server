namespace Musdis.FileService.Requests;

public record UploadTrackRequest(
    Guid TrackId,
    IFormFile File
);