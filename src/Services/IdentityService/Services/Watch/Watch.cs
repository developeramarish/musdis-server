namespace IdentityService.Services.Watch;


/// <inheritdoc cref="IWatch"/>
public class Watch : IWatch
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Today => DateTime.Today;
}