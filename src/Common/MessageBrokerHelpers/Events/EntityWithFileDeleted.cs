namespace Musdis.MessageBrokerHelpers.Events;

/// <summary>
///     Represents an event for an entity with a file being deleted.
/// </summary>
/// 
/// <param name="FileId">
///     The identifier of the file.
/// </param>
public sealed record EntityWithFileDeleted(Guid FileId);