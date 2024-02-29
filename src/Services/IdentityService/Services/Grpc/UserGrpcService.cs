using Grpc.Core;

using Microsoft.EntityFrameworkCore;

using Musdis.Common.GrpcProtos;
using Musdis.IdentityService.Data;

namespace Musdis.IdentityService.Services.Grpc;

/// <summary>
///     <see cref="UserService"/> server-side implementation. 
/// </summary>
public sealed class UserGrpcService : UserService.UserServiceBase
{
    private readonly IdentityServiceDbContext _dbContext;
    private readonly ILogger<UserGrpcService> _logger;

    public UserGrpcService(
        IdentityServiceDbContext dbContext,
        ILogger<UserGrpcService> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    ///     Retrieves user information.
    /// </summary>
    /// 
    /// <param name="request">
    ///     A request to get user info.
    /// </param>
    /// <param name="context">
    ///     gRPC context for a server-side call.
    /// </param>
    /// 
    /// <returns>
    ///     Information about user that was requested.
    /// </returns>
    /// <exception cref="RpcException">
    ///     Thrown when user not found.
    /// </exception>
    public override async Task<UserInfo> GetUserInfo(
        GetUserInfoRequest request,
        ServerCallContext context
    )
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, context.CancellationToken);
        if (user is null)
        {
            _logger.LogError("Cannot find User with Id = {Id}", request.Id);
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"User with Id = {{{request.Id}}} is not found."
            ));
        }

        _logger.LogInformation("Retrieving user data with Id = {Id}", user.Id);

        return new() { Id = user.Id, UserName = user.UserName };
    }

    public override async Task<UserInfos> GetUserInfos(
        GetUserInfosRequest request,
        ServerCallContext context
    )
    {
        if (request.Ids.Count == 0)
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Identifiers list cannot be empty"
            ));
        }

        var users = await _dbContext.Users
            .AsNoTracking()
            .Where(u => request.Ids.Contains(u.Id))
            .ToArrayAsync(context.CancellationToken);

        if (users.Length != request.Ids.Distinct().Count())
        {
            var invalidIds = string.Join(", ", request.Ids
                .Distinct()
                .Except(users.Select(u => u.Id))
            );
            _logger.LogError(
                "Cannot find Users with Ids = [{Ids}]",
                invalidIds
            );

            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Users with Ids = [{invalidIds}] are not found."
            ));
        }

        _logger.LogInformation(
            "Retrieving users data with Ids = {Ids}",
            users.Select(u => u.Id)
        );

        var result = new UserInfos();
        result.Users.AddRange(users.Select(u => new UserInfo
        {
            Id = u.Id,
            UserName = u.UserName
        }));

        return result;
    }
}