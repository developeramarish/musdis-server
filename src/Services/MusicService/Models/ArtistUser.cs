namespace Musdis.MusicService.Models;

/// <summary>
///     Represents model that contains info about <see cref="Models.Artist"/> and associated Users 
/// </summary>
/// <remarks>
///     User models specified in another service that handles authentication and authorization.
/// </remarks>
public class ArtistUser
{
    /// <summary>
    ///     The foreign key to <see cref="Models.Artist"/> table.
    /// </summary>
    public required Guid ArtistId { get; set; }

    /// <summary>
    ///     <see cref="Models.Artist"/> associated with this object.
    /// </summary>
    public Artist? Artist { get; set; }
    
    /// <summary>
    ///     The user identifier within the authentication and authorization service.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    ///     The username of user within the authentication and authorization service.
    /// </summary>
    public required string UserName { get; set; }
}