using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.User.Grpc.Proto;

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
