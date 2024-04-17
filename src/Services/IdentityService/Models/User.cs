using Microsoft.AspNetCore.Identity;

namespace Musdis.IdentityService.Models;

/// <summary>
///     The user of the application.
/// </summary>
public class User : IdentityUser 
{ 
    /// <summary>
    ///     The URL of the user's avatar.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    ///     The ID of the user's avatar in the file service.
    /// </summary>
    public Guid? AvatarFileId { get; set; }
}
