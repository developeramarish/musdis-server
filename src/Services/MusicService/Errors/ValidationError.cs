using Musdis.OperationResults;

namespace Musdis.MusicService.Errors;

/// <summary>
/// Represents an error indicating a validation failure, 
/// associated with HTTP status code 400 (Bad Request).
/// </summary>
/// <param name="description">The description of the error.</param>
public class ValidationError(string description) 
    : Error(StatusCodes.Status400BadRequest, description) { }