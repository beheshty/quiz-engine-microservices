using Contracts.Grpc.UserService.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using UserService.API.Models;

namespace UserService.Grpc.Services
{
    public class UserService : Users.UsersBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<GetUserInfoResponse> GetUserInfoById(GetUserInfoRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.UserId} not found."));
            }

            return new GetUserInfoResponse
            {
                User = new UserInfo()
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty
                }
            };
        }
    }
}
