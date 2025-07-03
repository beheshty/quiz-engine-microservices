using Contracts.Grpc.UserService.Proto;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Application.Grpc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsersGrpcClient(this IServiceCollection services, string grpcUrl)
        {
            services.AddGrpcClient<Users.UsersClient>((serviceProvider, options) =>
            {
                options.Address = new Uri(grpcUrl);
            });

            services.AddScoped<IUserGrpcClient, UserGrpcClient>();

            return services;
        }
    }
}
