using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;

namespace Musdis.ResponseHelpers.Errors;

/// <summary>
///     Represents a no content error, associated with status code 204.
/// </summary>
/// <remarks>
///     Technically it is not HTTP error, but should be used as invalid result of deletion operation.
/// </remarks>
public sealed class NoContentError : Error
{
    /// <summary>
    ///     HTTP response status code associated with this error.
    /// </summary>
    public int StatusCode => StatusCodes.Status204NoContent;

    /// <summary>
    ///     Initializes a new instance of the <see cref="NoContentError"/> class.
    /// </summary>
    /// 
    /// <param name="description">
    ///     A description providing additional information about the error.
    /// </param>
    public NoContentError(string description) : base(description) { }

    /// <inheritdoc cref="NoContentError.NoContentError(string)"/>
    public NoContentError() : this("No additional content to send.") { }
}