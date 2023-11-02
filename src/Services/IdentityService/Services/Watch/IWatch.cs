namespace IdentityService.Services.Watch;

/// <summary>
/// Service for providing current time.
/// </summary>
public interface IWatch
{
    /// <summary>
    /// Current date and time on this computer, expressed as the local time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Current date.
    /// </summary>
    DateTime Today { get; }
}