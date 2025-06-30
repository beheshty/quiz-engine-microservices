using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.User.Grpc.Proto;

namespace NotificationService.Application.Grpc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsersGrpcClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserGrpcOptions>(configuration.GetSection(UserGrpcOptions.SectionName));

            services.AddGrpcClient<Users.UsersClient>((serviceProvider, options) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<UserGrpcOptions>>().Value;
                options.Address = new Uri(settings.GrpcUrl);
            });

            services.AddScoped<IUserGrpcClient, UserGrpcClient>();

            return services;
        }
    }
}
