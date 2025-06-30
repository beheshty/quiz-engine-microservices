
using Shared.User.Grpc.Proto;

namespace NotificationService.Application.Grpc
{
    public class UserGrpcClient : IUserGrpcClient
    {
        private readonly Users.UsersClient _usersClient;

        public UserGrpcClient(Users.UsersClient usersClient)
        {
            _usersClient = usersClient;
        }

        public async Task<UserInfo> GetUserInfoAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userRequest = new GetUserInfoRequest()
            {
                UserId = userId.ToString(),
            };
            var result =  await _usersClient.GetUserInfoByIdAsync(userRequest, cancellationToken: cancellationToken);
            return result.User;
        }
    }
}
