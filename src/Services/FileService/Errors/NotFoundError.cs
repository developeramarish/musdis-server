using Results;

namespace FileService.Errors;

public sealed class NotFoundError : Error
{
    public NotFoundError(string description) 
        : base(404, description) { }
}