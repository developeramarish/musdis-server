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
/// <param name="Email">
///     The user's email address. 
/// </param>
public record UserReadDto(
    string Id,
    string UserName,
    string Email
)
{
    /// <summary>
    ///     Converts <paramref name="user"/> to <see cref="UserReadDto"/>.
    /// </summary>
    /// 
    /// <param name="user">
    ///     A user to convert.
    /// </param>
    /// 
    /// <returns>
    ///     A result of conversion. <br/>
    ///     On success: <see cref="Result{T}.Value"/> is <see cref="UserReadDto"/>. <br/>
    ///     On failure: <see cref="Result{T}.Error"/> is <see cref="Error"/> - when incorrect data passed.
    /// </returns>
    public static Result<UserReadDto> FromUser(User user)
    {
        if (user?.UserName is null || user?.Email is null)
        {
            return new Error(
                "Cannot convert user into DTO, null data provided"
            ).ToValueResult<UserReadDto>();
        }

        return new UserReadDto(
            user.Id,
            user.UserName,
            user.Email
        ).ToValueResult();
    }


    /// <summary>
    ///     Converts a collection of <paramref name="users"/> to <see cref="IEnumerable{T}"/> of <see cref="UserReadDto"/>.
    /// </summary>
    /// 
    /// <param name="users">
    ///     Users to convert.
    /// </param>
    /// 
    /// <returns>
    ///     A result of conversion. <br/>
    ///     On success: <see cref="Result{T}.Value"/> is <see cref="IEnumerable{T}"/> of <see cref="UserReadDto"/>. <br/>
    ///     On failure: <see cref="Result{T}.Error"/> is <see cref="Error"/> - when incorrect data passed.
    /// </returns>
    public static Result<IEnumerable<UserReadDto>> FromUsers(IEnumerable<User> users)
    {
        List<UserReadDto> dtos = [];

        foreach (var artist in users)
        {
            var result = FromUser(artist);
            if (result.IsFailure)
            {
                return new Error(
                    "Cannot convert users collection into user DTOs"
                ).ToValueResult<IEnumerable<UserReadDto>>();
            }

            dtos.Add(result.Value);
        }

        return dtos.AsEnumerable().ToValueResult();
    }
}