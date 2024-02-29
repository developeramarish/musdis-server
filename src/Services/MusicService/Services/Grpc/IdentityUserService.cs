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

    public async Task<Result<UserInfo>> GetUserInfoAsync(
        string userId,
        CancellationToken cancellationToken = default
    )
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
        catch (Exception ex)
        {
            return Result<UserInfo>.Failure(ex.Message);
        }
    }

    public async Task<Result<UserInfos>> GetUserInfosAsync(
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var request = new GetUserInfosRequest();
            request.Ids.AddRange(userIds);
            var userInfos = await _userServiceClient.GetUserInfosAsync(
                request,
                cancellationToken: cancellationToken
            );

            return userInfos.ToValueResult();
        }
        catch (RpcException ex) when (ex.StatusCode is StatusCode.InvalidArgument or StatusCode.NotFound)
        {
            return new ValidationError(ex.Message).ToValueResult<UserInfos>();
        }
        catch (Exception ex)
        {
            return Result<UserInfos>.Failure(ex.Message);
        }
    }
}
