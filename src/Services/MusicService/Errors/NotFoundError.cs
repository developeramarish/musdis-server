using Musdis.OperationResults;

namespace Musdis.MusicService.Errors;

/// <summary>
/// Represents content not found error with status code 404.
/// </summary>
/// <param name="description">Description of error.</param>
public class NotFoundError(string description)
    : Error(StatusCodes.Status404NotFound, description) { }