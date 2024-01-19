namespace Musdis.OperationResults;

/// <summary>
/// Represents an error in Results pattern.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Error"/> class with the specified error code and description.
/// </remarks>
/// <param name="code">The error code.</param>
/// <param name="description">The description of the error.</param>
public class Error(int code, string description)
{
    /// <summary>
    /// The error code.
    /// </summary>
    public int Code { get; private set; } = code;

    /// <summary>
    /// The description of the error.
    /// </summary>
    public string Description { get; private set; } = description;
}