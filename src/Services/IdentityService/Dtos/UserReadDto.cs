namespace Musdis.IdentityService.Dtos;

/// <summary>
///     Represents a DTO for reading user information.
/// </summary>
/// 
/// <param name="Id">
///     The unique identifier of the user. 
/// </param>
/// <param name="UserName">
///     The user's username. 
/// </param>
/// <param name="Email">
///     The user's email address. 
/// </param>
public record UserReadDto(
    string Id,
    string UserName,
    string Email
);