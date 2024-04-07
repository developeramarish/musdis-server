namespace Musdis.MessageBrokerHelpers.Events;

/// <summary>
///     Represents an event for a file being used.
/// </summary>
/// 
/// <param name="Id">
///     The identifier of the file.
/// </param>
public sealed record FileUsed(Guid Id);