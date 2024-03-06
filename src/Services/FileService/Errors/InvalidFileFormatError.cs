using Musdis.OperationResults;

namespace Musdis.FileService.Errors;

/// <summary>
///     Represents an error that occurs when the file format is invalid.
/// </summary>
public sealed class InvalidFileFormatError : Error
{
    public InvalidFileFormatError(string description) 
        : base(description) { }

    public InvalidFileFormatError() 
        : base("The file format is not supported.") { }
}