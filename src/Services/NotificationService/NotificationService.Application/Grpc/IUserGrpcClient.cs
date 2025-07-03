
using Contracts.Grpc.UserService.Proto;

namespace NotificationService.Application.Grpc
{
    public interface IUserGrpcClient
    {
        Task<UserInfo> GetUserInfoAsync(Guid userId, CancellationToken cancellationToken);
    }
}
