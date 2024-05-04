using Musdis.IdentityService.Exceptions;
using Musdis.IdentityService.Models;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;

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
/// <param name="AvatarUrl"> 
///     The link to the avatar file of the authenticated user.
/// </param>
/// <param name="Email">
///     The user's email address. 
/// </param>
public record UserDto(
    string Id,
    string UserName,
    string AvatarUrl,
    string Email
)
{
    /// <summary>
    ///     Converts <paramref name="user"/> to <see cref="UserDto"/>.
    /// </summary>
    /// 
    /// <param name="user">
    ///     A user to convert.
    /// </param>
    /// 
    /// <returns>
    ///     A <see cref="UserDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown when invalid data provided.
    /// </exception>
    public static UserDto FromUser(User user)
    {
        if (user?.UserName is null || user.Email is null || user.AvatarUrl is null)
        {
            throw new InvalidMethodCallException(
                "Cannot convert user into DTO, null data provided"
            );
        }

        return new(
            user.Id,
            user.UserName,
            user.AvatarUrl,
            user.Email
        );
    }


    /// <summary>
    ///     Converts a collection of <paramref name="users"/> to <see cref="IEnumerable{T}"/> of <see cref="UserDto"/>.
    /// </summary>
    /// 
    /// <param name="users">
    ///     Users to convert.
    /// </param>
    /// 
    /// <returns>
    ///     A collection of <see cref="UserDto"/>.
    /// </returns>
    /// <exception cref="InvalidMethodCallException">
    ///     Thrown when invalid data provided.
    /// </exception>
    public static IEnumerable<UserDto> FromUsers(IEnumerable<User> users)
    {
        return users.Select(a => FromUser(a));
    }
}