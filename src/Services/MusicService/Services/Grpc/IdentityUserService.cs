using Grpc.Core;

using Musdis.Common.GrpcProtos;
using Musdis.OperationResults;
using Musdis.OperationResults.Extensions;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.MusicService.Services.Grpc;

/// <inheritdoc cref="IIdentityUserService"/>
public sealed class IdentityUserService : IIdentityUserService
{
    private readonly UserService.UserServiceClient _userServiceClient;

    public IdentityUserService(UserService.UserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    public async Task<Result<UserInfo>> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var userInfo = await _userServiceClient.GetUserInfoAsync(
                new GetUserInfoRequest { Id = userId },
                cancellationToken: cancellationToken
            );

            return userInfo.ToValueResult();
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return new NotFoundError(
                $"Cannot find user with Id = {{{userId}}}"
            ).ToValueResult<UserInfo>();
        }
        catch(Exception ex)
        {
            return Result<UserInfo>.Failure(ex.Message);
        }
    }
}
