using Musdis.OperationResults;

namespace Musdis.MusicService.Errors;

/// <summary>
///     Represents content no content error with status code 204.
/// </summary>
/// <remarks>
///     Technically it is not HTTP error, but should be used as invalid result of deleting operation.
/// </remarks>
/// 
/// <param name="description">
///     The description of the error.
/// </param>
public class NoContentError(string description)
    : Error(StatusCodes.Status204NoContent, description) { }