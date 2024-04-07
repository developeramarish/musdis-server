namespace Musdis.FileService.MessageBroker.Commands;

/// <summary>
///     Represents a command for deleting a file scheduled.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the file.
/// </param>
public sealed record DeleteFileScheduled(Guid Id);