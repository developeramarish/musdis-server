using Musdis.OperationResults;

namespace Musdis.FileService.Errors;

public sealed class NotFoundError(string description) 
    : Error(StatusCodes.Status404NotFound, description) { }