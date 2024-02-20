namespace Musdis.MusicService.Exceptions;

/// <summary>
///     Represents an exception that is thrown when a method is invoked
///     in an incorrect context or with invalid data.
/// </summary>
public class InvalidMethodCallException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidMethodCallException"/> class.
    /// </summary>
    public InvalidMethodCallException() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidMethodCallException"/> class
    ///     with a specified error message.
    /// </summary>
    /// 
    /// <param name="message">
    ///     The error message that explains the reason for the exception.
    /// </param>
    public InvalidMethodCallException(string message) 
        : base(message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidMethodCallException"/> class
    ///     with a specified error message and a reference to the inner exception that is the cause of
    ///     this exception.
    /// </summary>
    /// 
    /// <param name="message">
    ///     The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception. If the <paramref name="innerException"/>
    ///     parameter is not null, the current exception is raised in a catch block that handles the
    ///     inner exception.
    /// </param>
    public InvalidMethodCallException(string message, Exception innerException) 
        : base(message, innerException) { }
}