using Results;

namespace FileService.Errors;

public sealed class NotFoundError(string description) 
    : Error(404, description) { }