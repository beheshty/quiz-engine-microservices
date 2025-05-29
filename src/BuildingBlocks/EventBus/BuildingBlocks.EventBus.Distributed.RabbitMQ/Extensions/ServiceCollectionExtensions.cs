using BuildingBlocks.EventBus.Abstraction.Distributed;
using BuildingBlocks.EventBus.Distributed.RabbitMQ.Attributes;
using BuildingBlocks.EventBus.Distributed.RabbitMQ.Configuration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.EventBus.Distributed.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRabbitMqMassTransitEventBus(
                        this IServiceCollection services,
                        Assembly assembly,
                        Action<RabbitMqOptions> configureOptions)
        {

            var options = new RabbitMqOptions();
            configureOptions?.Invoke(options);

            services.AddScoped(typeof(RabbitMqEventConsumer<>));
            services.AddScoped<IDistributedEventBus, RabbitMqEventBus>();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumers(assembly);

                cfg.UsingRabbitMq((context, busCfg) =>
                {
                    busCfg.Host(new Uri(options.HostUrl), h =>
                    {
                        if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
                        {
                            h.Username(options.Username);
                            h.Password(options.Password);
                        }
                    });

                    busCfg.ConfigureWithAttributes(cfg, context, assembly);
                });
            });

            return services;
        }

        private static void ConfigureWithAttributes(this IRabbitMqBusFactoryConfigurator rfc, IBusRegistrationConfigurator brc,
                                                   IRegistrationContext context, Assembly assembly)
        {
            var messageTypesWithAttributes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<DistributedMessageAttribute>() != null);

            foreach (var messageType in messageTypesWithAttributes)
            {
                var attribute = messageType.GetCustomAttribute<DistributedMessageAttribute>();

                var consumerType = typeof(RabbitMqEventConsumer<>).MakeGenericType(messageType);
                if (consumerType == null)
                    continue;
                brc.AddConsumer(consumerType);

                rfc.ReceiveEndpoint(attribute.Subscription, e =>
                {
                    e.Bind(attribute.Destination, x =>
                    {
                        x.ExchangeType = attribute.DistributionStrategy.ToString().ToLower();
                        x.RoutingKey = messageType.FullName;
                    });

                    e.ConfigureConsumer(context, consumerType);
                });
            }
        }
    }
}
